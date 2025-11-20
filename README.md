Employee Task Tracker

A full-stack internal Employee Task Tracking System where admins can create and assign tasks, and employees can view and update their task progress.

Built with .NET 8 Web API, Entity Framework Core, React (Vite + Tailwind CSS).

Features
Admin

Create employees

Create tasks

Assign tasks to employees

View all tasks

Employee

View assigned tasks

Update task status: Not Started → In Progress → Completed

Add comments to tasks (optional)

Tech Stack

Backend: .NET 8 Web API, Entity Framework Core

Database: SQL Server / SQLite

Frontend: React (Vite) + Tailwind CSS

HTTP Client: Axios

Project Structure
Backend/
 ├─ Controllers/
 ├─ Models/
 ├─ Services/
 └─ Program.cs / Startup.cs

Frontend/
 ├─ src/
 │  ├─ api/
 │  ├─ components/
 │  ├─ pages/
 │  └─ main.tsx
 └─ index.html

Prerequisites

.NET 8 SDK

Node.js & npm

SQL Server or SQLite installed locally

Setup Instructions
Backend

Open the backend folder in terminal/IDE.

Restore dependencies:

dotnet restore


Apply database migrations:

dotnet ef database update


Run the backend:

dotnet run


API should now be running at https://localhost:5001 or similar.

Frontend

Navigate to the frontend folder:

cd EventTrackerApp


Install dependencies:

npm install


Start the development server:

npm run dev


React app should now be running at http://localhost:5173 (or the port Vite shows).

Login Credentials

Admin: Use the seeded admin account if provided, or create via backend.

Employee: Use the seeded employee accounts or create via Admin.

Usage

Open frontend in browser.

Login as Admin or Employee.

Admin can create tasks and assign them.

Employee can view their tasks, update status, and add comments.

API Endpoints
Method	Endpoint	Description
POST	/auth/login	Login user
GET	/tasks	Get all tasks (Admin) / Assigned tasks (Employee)
POST	/tasks	Create task (Admin only)
PUT	/tasks/{id}/status	Update task status (Employee)
POST	/tasks/{id}/comments	Add comment to task
Notes

Ensure backend is running before starting frontend.

Cookies are used for authentication (no JWT).

Tailwind CSS handles the frontend styling.
