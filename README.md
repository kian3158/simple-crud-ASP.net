# SchoolAPI CRUD Application

A simple CRUD backend application for managing students, courses, and teachers.

## Overview
This application allows users to:
- Add, view, edit, and delete Students, Courses, and Teachers.
- Handle many-to-many relationships between Students and Courses.
- Handle one-to-many relationships between Teachers and Courses.

## Technologies Used
- **Backend:** C#, ASP.NET Core 8 Web API
- **Database:** SQL Server
- **ORM:** Entity Framework Core

## Setup Instructions
1. Clone the repository:
```bash
git clone https://github.com/kian3158/simple-crud-ASP.net.git
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Configure the database connection string in `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SchoolDb;Trusted_Connection=True;"
}
```

4. Apply migrations and create the database:
```bash
dotnet ef database update
```

5. (Optional) Seed the database using SQL script located at `Data/SeedData.sql`.

6. Run the application:
```bash
dotnet run
```

7. Access the API documentation via Swagger at `https://localhost:5266/swagger`.

## API Endpoints
### Students
- `GET /api/students` – Get all students
- `GET /api/students/{id}` – Get student by ID
- `POST /api/students` – Add a new student
- `PUT /api/students/{id}` – Update student details
- `DELETE /api/students/{id}` – Delete a student

### Courses
- `GET /api/courses` – Get all courses
- `GET /api/courses/{id}` – Get course by ID
- `POST /api/courses` – Add a new course
- `PUT /api/courses/{id}` – Update course details
- `DELETE /api/courses/{id}` – Delete a course

### Teachers
- `GET /api/teachers` – Get all teachers
- `GET /api/teachers/{id}` – Get teacher by ID
- `POST /api/teachers` – Add a new teacher
- `PUT /api/teachers/{id}` – Update teacher details
- `DELETE /api/teachers/{id}` – Delete a teacher

## Example JSON Payloads
### Student
```json
{
  "name": "Student A",
  "email": "studenta@example.com",
  "dateOfBirth": "2000-01-01T00:00:00",
  "phoneNumber": "1234567890"
}
```

### Course
```json
{
  "courseName": "Course A",
  "teacherId": 1
}
```

### Teacher
```json
{
  "name": "Teacher A",
  "email": "teachera@example.com",
  "phoneNumber": "0987654321"
}
```

## Database Schema
- **Students**: `Id`, `Name`, `DateOfBirth`, `Email`, `PhoneNumber`
- **Teachers**: `Id`, `Name`, `Email`, `PhoneNumber`
- **Courses**: `CourseId`, `CourseName`, `TeacherId`
- **StudentCourse**: Junction table for Students and Courses

### Database Initialization with DbInitializer

The application includes a `DbInitializer` class that seeds initial data when the application starts. This ensures the database contains sample students, teachers, and courses for testing purposes.  

To use it:

1. The `DbInitializer.Initialize(context)` method is called in `Program.cs` during app startup.
2. It will create and populate:
   - 10 students
   - 10 teachers
   - 5 courses
   - StudentCourse relationships for many-to-many linking
3. You can adjust the sample data inside `DbInitializer.cs` if needed.

## Seed Data
Seed data is located at `Data/SeedData.sql` and contains:
- 10 Students
- 10 Teachers
- 5 Courses
