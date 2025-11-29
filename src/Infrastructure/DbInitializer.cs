using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolApi.Domain.Models;

namespace SchoolApi.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(SchoolContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Admin
            var adminEmail = "admin@example.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "+1234567890"
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Teachers
            if (!context.Teachers.Any())
            {
                var teachers = new Teacher[]
                {
                    new Teacher { Name = "Teacher 1", Email = "teacher1@example.com", PhoneNumber = "+98-21-1001" },
                    new Teacher { Name = "Teacher 2", Email = "teacher2@example.com", PhoneNumber = "+98-21-1002" },
                    new Teacher { Name = "Teacher 3", Email = "teacher3@example.com", PhoneNumber = "+98-21-1003" },
                };
                context.Teachers.AddRange(teachers);
                await context.SaveChangesAsync();

                foreach (var teacher in teachers)
                {
                    if (await userManager.FindByEmailAsync(teacher.Email) == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = teacher.Email,
                            Email = teacher.Email,
                            PhoneNumber = teacher.PhoneNumber
                        };
                        await userManager.CreateAsync(user, "Teacher123!");
                        await userManager.AddToRoleAsync(user, "Teacher");

                        teacher.ApplicationUserId = user.Id;
                    }
                }
                await context.SaveChangesAsync();
            }

            // Students
            if (!context.Students.Any())
            {
                var students = new Student[]
                {
                    new Student { Name = "Student 1", DateOfBirth = new DateTime(2000,1,1), Email = "student1@example.com", PhoneNumber = "+98-912-0001" },
                    new Student { Name = "Student 2", DateOfBirth = new DateTime(2000,2,2), Email = "student2@example.com", PhoneNumber = "+98-912-0002" },
                    new Student { Name = "Student 3", DateOfBirth = new DateTime(2000,3,3), Email = "student3@example.com", PhoneNumber = "+98-912-0003" },
                };
                context.Students.AddRange(students);
                await context.SaveChangesAsync();

                foreach (var student in students)
                {
                    if (await userManager.FindByEmailAsync(student.Email) == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = student.Email,
                            Email = student.Email,
                            PhoneNumber = student.PhoneNumber
                        };
                        await userManager.CreateAsync(user, "Student123!");
                        await userManager.AddToRoleAsync(user, "Student");

                        student.ApplicationUserId = user.Id;
                    }
                }
                await context.SaveChangesAsync();
            }

            // Courses
            if (!context.Courses.Any())
            {
                var teachers = context.Teachers.ToArray();
                var courses = new Course[]
                {
                    new Course { CourseName = "Math", TeacherId = teachers[0].Id },
                    new Course { CourseName = "Physics", TeacherId = teachers[1].Id },
                    new Course { CourseName = "Chemistry", TeacherId = teachers[2].Id },
                    new Course { CourseName = "English", TeacherId = teachers[0].Id },
                    new Course { CourseName = "Farsi", TeacherId = teachers[1].Id },
                };
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }

            // StudentCourses
            if (!context.StudentCourses.Any())
            {
                var students = context.Students.ToArray();
                var courses = context.Courses.ToArray();
                var enrollments = new StudentCourse[]
                {
                    new StudentCourse { StudentId = students[0].Id, CourseId = courses[0].CourseId },
                    new StudentCourse { StudentId = students[0].Id, CourseId = courses[2].CourseId },
                    new StudentCourse { StudentId = students[1].Id, CourseId = courses[1].CourseId },
                    new StudentCourse { StudentId = students[2].Id, CourseId = courses[3].CourseId },
                };
                context.StudentCourses.AddRange(enrollments);
                await context.SaveChangesAsync();
            }
        }
    }
}
