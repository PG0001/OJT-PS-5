# Employee Task Tracker

A full-stack internal Employee Task Tracking System where admins can create and assign tasks, and employees can view and update their task progress.

Built with .NET 8 Web API, Entity Framework Core, React (Vite + Tailwind CSS).

---

## Features

### Admin

* Create employees
* Create tasks
* Assign tasks to employees
* View all tasks

### Employee

* View assigned tasks
* Update task status: Not Started ➞ In Progress ➞ Completed
* Add comments to tasks (optional)

---

## Tech Stack

* Backend: .NET 8 Web API, Entity Framework Core
* Database: SQL Server / SQLite
* Frontend: React (Vite) + Tailwind CSS
* HTTP Client: Axios

---

## Project Structure

```
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
```

---

## Prerequisites

* .NET 8 SDK
* Node.js & npm
* SQL Server or SQLite installed locally

---

## Setup Instructions

### Backend

1. Open the backend folder in terminal/IDE.
2. Restore dependencies:

```
dotnet restore
```

3. Apply database migrations:

```
dotnet ef database update
```

4. Run the backend:

```
dotnet run
```

* API should now be running at `https://localhost:5001` (or configured port).

### Frontend

1. Open the frontend folder in terminal.
2. Install dependencies:

```
npm install
```

3. Run the development server:

```
npm run dev
```

* Frontend should now be running at `http://localhost:5173` (or Vite’s assigned port).

---

## Usage

1. Open the app in your browser.
2. Login as **Admin** or **Employee** using credentials seeded in the database (or create users if backend supports it).
3. **Admin Dashboard:**

   * Create tasks, assign employees, view all tasks.
4. **Employee Dashboard:**

   * View assigned tasks, update status, add comments.

---

## Notes

* All API requests are made with **Axios** and support **cookie-based authentication**.
* Tailwind CSS is used for styling. You can customize colors in `index.css`.
* The project follows a **clean architecture** approach for backend.

---

## Troubleshooting

* **CORS issues:** Ensure backend and frontend ports are configured properly.
* **Database connection errors:** Check your `appsettings.json` for correct connection string.
* **Port conflicts:** Change the ports in backend `launchSettings.json` or frontend `vite.config.ts`.
