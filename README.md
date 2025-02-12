# Team Management System

## Overview

Team Management System is a web application built with ASP.NET Core MVC and Entity Framework Core, designed to manage teams, track project progress, and monitor task deadlines. The system enables users to assign employees to projects, track work hours, and manage user authentication securely.

### Features

* Project & Task Management: Track assigned projects, monitor deadlines, and calculate total work hours.

* Employee Workload Tracking: View individual and team workload based on assigned tasks.

* Data Grouping & Sorting: Display hierarchical relationships (e.g., modules grouped by projects).

* CRUD Operations: Create, read, update, and delete records across all tables.

* Role-Based Access Control: Admin users can manage user accounts and permissions.

* Database Integration: Uses SQLite with Entity Framework Core for database management.

* MVC Architecture: Implements Model-View-Controller pattern for structured code organization.

### Authentication & Security:

* User authentication with MD5 password hashing.

* Restricted access to database-related views for unauthorized users.

## Database Schema
![image alt](https://github.com/KamilMleczko/TeamMngtApplication/blob/master/database.png?raw=true)
