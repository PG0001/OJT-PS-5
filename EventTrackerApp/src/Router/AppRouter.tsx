import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "../Components/Login";
import AdminDashboard from "../Components/Admin-Dashboard/AdminDashboard";
import EmployeeDashboard from "../Components/Employee-Dashboard/EmployeeDashboard";

export default function AppRouter() {
  const role = localStorage.getItem("role");

  return (
    <BrowserRouter>
      <Routes>

        {/* Default → Login */}
        <Route path="/" element={<Login />} />

        {/* Admin Only */}
        <Route
          path="/admin-dashboard"
          element={
            role === "Admin" ? (
              <AdminDashboard />
            ) : (
              <Navigate to="/" />
            )
          }
        />

        {/* Employee Only */}
        <Route
          path="/employee-dashboard"
          element={
            role === "User" ? (
              <EmployeeDashboard />
            ) : (
              <Navigate to="/" />
            )
          }
        />

        {/* Unknown routes → Login */}
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}
