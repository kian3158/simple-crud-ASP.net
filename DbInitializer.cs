using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolApi.Models;

namespace SchoolApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Admin
            var adminEmail = "admin@example.com";
            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "+1234567890"
                };
                userManager.CreateAsync(admin, "Admin123!").Wait();
                userManager.AddToRoleAsync(admin, "Admin").Wait();
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
                context.SaveChanges();

                foreach (var teacher in teachers)
                {
                    if (userManager.FindByEmailAsync(teacher.Email).Result == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = teacher.Email,
                            Email = teacher.Email,
                            PhoneNumber = teacher.PhoneNumber
                        };
                        userManager.CreateAsync(user, "Teacher123!").Wait();
                        userManager.AddToRoleAsync(user, "Teacher").Wait();

                        teacher.ApplicationUserId = user.Id;
                    }
                }
                context.SaveChanges();
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
                context.SaveChanges();

                foreach (var student in students)
                {
                    if (userManager.FindByEmailAsync(student.Email).Result == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = student.Email,
                            Email = student.Email,
                            PhoneNumber = student.PhoneNumber
                        };
                        userManager.CreateAsync(user, "Student123!").Wait();
                        userManager.AddToRoleAsync(user, "Student").Wait();

                        student.ApplicationUserId = user.Id;
                    }
                }
                context.SaveChanges();
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
                context.SaveChanges();
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
                context.SaveChanges();
            }
        }
    }
}
