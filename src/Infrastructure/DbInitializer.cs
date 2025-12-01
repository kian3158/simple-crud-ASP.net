using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolApi.Infrastructure
{
    public static class DbInitializer
    {
        // Main entry used by Program.cs
        public static void Initialize(SchoolContext context, IServiceProvider serviceProvider, ILogger? logger = null)
        {
            try
            {
                logger?.LogInformation("Starting DB migration/initialization...");
                // apply migrations so Identity tables exist
                context.Database.Migrate();
                logger?.LogInformation("Database migrated/applied successfully.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to migrate database.");
                throw;
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    logger?.LogInformation("Creating role '{Role}'", role);
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Admin (single user)
            var adminEmail = "admin@example.com";
            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "+1234567890"
                };
                var res = userManager.CreateAsync(admin, "Admin123!").GetAwaiter().GetResult();
                if (res.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                    logger?.LogInformation("Seeded admin user: {Email}", adminEmail);
                }
                else
                {
                    logger?.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", res.Errors.Select(e => e.Description)));
                }
            }

            // TEACHERS: ensure Identity users exist first, then create Teacher entities with ApplicationUserId set
            if (!context.Teachers.Any())
            {
                var teacherInfos = Enumerable.Range(1, 10).Select(i => new
                {
                    Name = $"Teacher {i}",
                    Email = $"teacher{i}@example.com",
                    Phone = $"+98-21-{1000 + i:D4}"
                }).ToArray();

                foreach (var info in teacherInfos)
                {
                    // ensure Identity user exists
                    var user = userManager.FindByEmailAsync(info.Email).Result;
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Email,
                            Email = info.Email,
                            PhoneNumber = info.Phone
                        };
                        var createResult = userManager.CreateAsync(user, "Teacher123!").GetAwaiter().GetResult();
                        if (!createResult.Succeeded)
                        {
                            logger?.LogWarning("Failed to create Identity user for teacher {Email}: {Errors}", info.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                            continue; // skip this teacher (so we don't insert a teacher with null user)
                        }
                        userManager.AddToRoleAsync(user, "Teacher").Wait();
                    }

                    // create Teacher entity with ApplicationUserId assigned
                    var teacher = new Teacher
                    {
                        Name = info.Name,
                        Email = info.Email,
                        PhoneNumber = info.Phone,
                        ApplicationUserId = user.Id
                    };
                    context.Teachers.Add(teacher);
                }

                context.SaveChanges();
                logger?.LogInformation("Seeded teachers and corresponding Identity users (10).");
            }

            // STUDENTS: same approach as teachers (10)
            if (!context.Students.Any())
            {
                var studentInfos = Enumerable.Range(1, 10).Select(i => new
                {
                    Name = $"Student {i}",
                    Dob = new DateTime(2000, (i % 12) + 1, Math.Min(28, i)),
                    Email = $"student{i}@example.com",
                    Phone = $"+98-912-{1000 + i:D4}"
                }).ToArray();

                foreach (var info in studentInfos)
                {
                    var user = userManager.FindByEmailAsync(info.Email).Result;
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Email,
                            Email = info.Email,
                            PhoneNumber = info.Phone
                        };
                        var createResult = userManager.CreateAsync(user, "Student123!").GetAwaiter().GetResult();
                        if (!createResult.Succeeded)
                        {
                            logger?.LogWarning("Failed to create Identity user for student {Email}: {Errors}", info.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                            continue; // skip this student
                        }
                        userManager.AddToRoleAsync(user, "Student").Wait();
                    }

                    var student = new Student
                    {
                        Name = info.Name,
                        DateOfBirth = info.Dob,
                        Email = info.Email,
                        PhoneNumber = info.Phone,
                        ApplicationUserId = user.Id
                    };
                    context.Students.Add(student);
                }

                context.SaveChanges();
                logger?.LogInformation("Seeded students and corresponding Identity users (10).");
            }

            // COURSES (5 courses)
            if (!context.Courses.Any())
            {
                var teachers = context.Teachers.ToArray();
                if (teachers.Length >= 1)
                {
                    // pick 5 course names and assign to teachers round-robin
                    var courseNames = new[] { "Math", "Physics", "Chemistry", "English", "Farsi" };
                    var courses = courseNames.Select((name, idx) => new Course
                    {
                        CourseName = name,
                        TeacherId = teachers[idx % teachers.Length].Id
                    }).ToArray();

                    context.Courses.AddRange(courses);
                    context.SaveChanges();
                    logger?.LogInformation("Seeded courses (5).");
                }
                else
                {
                    logger?.LogWarning("Not enough seeded teachers to create courses. Teachers count: {Count}", teachers.Length);
                }
            }

            if (!context.StudentCourses.Any())
            {
                var students = context.Students.ToArray();
                var courses = context.Courses.ToArray();
                if (students.Length > 0 && courses.Length > 0)
                {
                    // create a few deterministic enrollments so dataset is sensible
                    var enrollments = new System.Collections.Generic.List<StudentCourse>();
                    // give first 5 students 2 courses each, others 1 course
                    for (int i = 0; i < Math.Min(5, students.Length); i++)
                    {
                        enrollments.Add(new StudentCourse { StudentId = students[i].Id, CourseId = courses[i % courses.Length].CourseId });
                        enrollments.Add(new StudentCourse { StudentId = students[i].Id, CourseId = courses[(i + 1) % courses.Length].CourseId });
                    }
                    for (int i = 5; i < students.Length; i++)
                    {
                        enrollments.Add(new StudentCourse { StudentId = students[i].Id, CourseId = courses[i % courses.Length].CourseId });
                    }

                    context.StudentCourses.AddRange(enrollments);
                    context.SaveChanges();
                    logger?.LogInformation("Seeded student enrollments.");
                }
                else
                {
                    logger?.LogWarning("Not enough students/courses to create student enrollments. Students: {S}, Courses: {C}", context.Students.Count(), context.Courses.Count());
                }
            }

            logger?.LogInformation("Database initialization complete.");
        }

        // Back-compat shim: keep the 2-arg overload so older code still works
        public static void Initialize(SchoolContext context, IServiceProvider serviceProvider)
        {
            Initialize(context, serviceProvider, null);
        }
    }
}
