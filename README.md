# School Management API

A CRUD application to manage Students, Courses, and Teachers with secure authentication and role-based access control. Built with ASP.NET Core and SQL Server.

## Overview

This application allows users to add, view, edit, and delete Students, Courses, and Teachers. Users are assigned roles that determine what actions they can perform.

## Roles and Permissions

### Admin
- Full control over all resources:
  - Students: create, edit, delete, list all, view individual details
  - Teachers: create, edit, delete, list all, view individual details
  - Courses: create, edit, delete, list all, view enrolled students
- Can assign or change roles of other users

### Teacher
- Can only view and manage courses assigned to them
- Can view students enrolled in their courses
- Can view own profile using `/teachers/me`
- Cannot modify other teachers, students, or courses not assigned to them

### Student
- Can view own profile using `/students/me`
- Can view the courses they are enrolled in
- Cannot modify any other data

## Role Assignment and Validation

- When a user is created (via Admin), they are assigned a role: Admin, Teacher, or Student.
- Roles are stored in the Identity system (`AspNetRoles`, `AspNetUserRoles`).
- Role validation is enforced on every request through JWT claims.
- Endpoints use `[Authorize(Roles = "...")]` to restrict access:
  - Admin endpoints: only Admins
  - Teacher endpoints: only the assigned Teacher
  - Student endpoints: only the authenticated Student

## API Endpoints

### Students
- `GET /api/students` (Admin) - list all students
- `POST /api/students` (Admin) - create student
- `PUT /api/students/{id}` (Admin) - edit student
- `DELETE /api/students/{id}` (Admin) - delete student
- `GET /api/students/me` (Student) - view own profile

### Teachers
- `GET /api/teachers` (Admin) - list all teachers
- `POST /api/teachers` (Admin) - create teacher
- `PUT /api/teachers/{id}` (Admin) - edit teacher
- `DELETE /api/teachers/{id}` (Admin) - delete teacher
- `POST /api/teachers/{id}/role` (Admin) - change role
- `GET /api/teachers/me` (Teacher) - view own profile

### Courses
- `GET /api/courses` (Admin) - list all courses
- `GET /api/courses/my` (Teacher) - view own courses
- `POST /api/courses` (Admin) - create course
- `PUT /api/courses/{id}` (Admin, Teacher) - edit course (Teacher can only edit own courses)
- `DELETE /api/courses/{id}` (Admin) - delete course
- `GET /api/courses/{id}/students` (Admin, Teacher, Student) - list students in a course (Student can only view courses they are enrolled in)

## Technologies
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- ASP.NET Identity





# Database Setup Scripts

This project includes two helper scripts to ensure every developer starts with the **same database schema and the same seed data**, without manual EF Core commands.



## 1. apply-migrations

**Purpose:**  
Apply all pending Entity Framework Core migrations to your local SQL Server database.

**Use this when:**
- You're running the project for the first time  
- You pulled changes that include DB updates  
- EF Core complains about missing tables/columns  
- Basically: **“Make my database not broken.”**

**What it does:**
- Runs `dotnet ef database update` behind the scenes  
- Ensures your database schema matches the project’s latest state  


## 2. seed-database

**Purpose:**  
Populate the database with initial demo/test data so the API works out of the box.

**This script creates:**
- **10 teachers**  
- **10 students**  
- **5 starter courses**  
- Default identity roles (if missing)



## Workflow Example

```bash
# 1. Make sure your schema is up to date
./src/Infrastructure/Scripts/apply-migrations

# 2. Seed the database with initial data
./src/Infrastructure/Scripts/seed-database
