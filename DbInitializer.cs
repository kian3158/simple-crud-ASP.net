using System;
using System.Linq;
using SchoolApi.Models; 

namespace SchoolApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            
            context.Database.EnsureCreated();
            
            if (context.Teachers.Any())
            {
                return; 
            }

            // Seed Teachers
            var teachers = new Teacher[]
            {
                new Teacher { Name = "Teacher 1", Email = "teacher1@example.com", PhoneNumber = "+98-21-1001" },
                new Teacher { Name = "Teacher 2", Email = "teacher2@example.com", PhoneNumber = "+98-21-1002" },
                new Teacher { Name = "Teacher 3", Email = "teacher3@example.com", PhoneNumber = "+98-21-1003" },
                new Teacher { Name = "Teacher 4", Email = "teacher4@example.com", PhoneNumber = "+98-21-1004" },
                new Teacher { Name = "Teacher 5", Email = "teacher5@example.com", PhoneNumber = "+98-21-1005" },
                new Teacher { Name = "Teacher 6", Email = "teacher6@example.com", PhoneNumber = "+98-21-1006" },
                new Teacher { Name = "Teacher 7", Email = "teacher7@example.com", PhoneNumber = "+98-21-1007" },
                new Teacher { Name = "Teacher 8", Email = "teacher8@example.com", PhoneNumber = "+98-21-1008" },
                new Teacher { Name = "Teacher 9", Email = "teacher9@example.com", PhoneNumber = "+98-21-1009" },
                new Teacher { Name = "Teacher 10", Email = "teacher10@example.com", PhoneNumber = "+98-21-1010" },
            };

            context.Teachers.AddRange(teachers);
            context.SaveChanges();

            // Seed Courses
            var courses = new Course[]
            {
                new Course { CourseName = "Math", TeacherId = teachers[0].Id },
                new Course { CourseName = "Physics", TeacherId = teachers[1].Id },
                new Course { CourseName = "Chemistry", TeacherId = teachers[2].Id },
                new Course { CourseName = "English", TeacherId = teachers[3].Id },
                new Course { CourseName = "Farsi", TeacherId = teachers[4].Id },
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Seed Students
            var students = new Student[]
            {
                new Student { Name = "Student 1", DateOfBirth = new DateTime(2000,1,1), Email = "student1@example.com", PhoneNumber = "+98-912-0001" },
                new Student { Name = "Student 2", DateOfBirth = new DateTime(2000,2,2), Email = "student2@example.com", PhoneNumber = "+98-912-0002" },
                new Student { Name = "Student 3", DateOfBirth = new DateTime(2000,3,3), Email = "student3@example.com", PhoneNumber = "+98-912-0003" },
                new Student { Name = "Student 4", DateOfBirth = new DateTime(2000,4,4), Email = "student4@example.com", PhoneNumber = "+98-912-0004" },
                new Student { Name = "Student 5", DateOfBirth = new DateTime(2000,5,5), Email = "student5@example.com", PhoneNumber = "+98-912-0005" },
                new Student { Name = "Student 6", DateOfBirth = new DateTime(2000,6,6), Email = "student6@example.com", PhoneNumber = "+98-912-0006" },
                new Student { Name = "Student 7", DateOfBirth = new DateTime(2000,7,7), Email = "student7@example.com", PhoneNumber = "+98-912-0007" },
                new Student { Name = "Student 8", DateOfBirth = new DateTime(2000,8,8), Email = "student8@example.com", PhoneNumber = "+98-912-0008" },
                new Student { Name = "Student 9", DateOfBirth = new DateTime(2000,9,9), Email = "student9@example.com", PhoneNumber = "+98-912-0009" },
                new Student { Name = "Student 10", DateOfBirth = new DateTime(2000,10,10), Email = "student10@example.com", PhoneNumber = "+98-912-0010" },
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            // Seed StudentCourses (Enrollments)
            var enrollments = new StudentCourse[]
            {
                new StudentCourse { StudentId = students[0].Id, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[0].Id, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[1].Id, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[1].Id, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[2].Id, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[2].Id, CourseId = courses[3].CourseId },
                new StudentCourse { StudentId = students[3].Id, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[3].Id, CourseId = courses[4].CourseId },
                new StudentCourse { StudentId = students[4].Id, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[4].Id, CourseId = courses[4].CourseId },
                new StudentCourse { StudentId = students[5].Id, CourseId = courses[3].CourseId },
                new StudentCourse { StudentId = students[6].Id, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[6].Id, CourseId = courses[4].CourseId },
                new StudentCourse { StudentId = students[7].Id, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[8].Id, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[9].Id, CourseId = courses[3].CourseId },
            };

            context.StudentCourses.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
