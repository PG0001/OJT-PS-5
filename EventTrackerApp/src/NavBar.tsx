import api from "./api/api";

interface NavBarProps {
  name: string;
  onLogout?: () => void; // optional, you can handle it here too
}

export default function NavBar({ name, onLogout }: NavBarProps) {
  const handleLogout = async () => {
    try {
      await api.post("/Auth/logout"); // backend clears the cookie
      localStorage.clear();           // clear UI info
      window.location.href = "/login";
      if (onLogout) onLogout();
    } catch (err) {
      console.error(err);
      alert("Logout failed");
    }
  };

  return (
    <header className="w-full bg-gray-800 shadow-md">
      <nav className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <h1 className="text-xl font-semibold text-gray-100">{name}</h1>

        <button
          onClick={handleLogout}
          className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
        >
          Logout
        </button>
      </nav>
    </header>
  );
}
