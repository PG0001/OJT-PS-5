import { useEffect, useState } from "react";
import api from "../../api/api";
import NavBar from "../../NavBar";

// ---------- INTERFACES ----------
interface Task {
  id: number;
  title: string;
  description: string;
  status: string;
  AssignedTo?: number | null;
}

interface User {
  id: number;
  name: string;
  role: string;
}

// ---------- COMPONENT ----------
export default function AdminDashboard() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [users, setUsers] = useState<User[]>([]);

  const [title, setTitle] = useState("");
  const [desc, setDesc] = useState("");
  const [priority, setPriority] = useState("");
  const [duedate, setDuedate] = useState("");

  const [assignPopup, setAssignPopup] = useState(false);
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);
  const [selectedUser, setSelectedUser] = useState("");

  const [showCreateTask, setShowCreateTask] = useState(false);
  const [adminName, setAdminName] = useState("");

  // ---------- CHECK AUTH & FETCH DATA ----------
  useEffect(() => {
    const init = async () => {
      try {
        const res = await api.get("/Auth/Check");
        setAdminName(res.data.name || "Admin");

        await fetchTasks();
        await fetchUsers();
      } catch (err) {
        console.warn("Not authenticated", err);
      }
    };

    init();
  }, []);

  // ---------- FETCH TASKS ----------
  const fetchTasks = async () => {
    try {
      const res = await api.get("/TaskItem");
      const formatted = res.data.map((t: any) => ({
        id: t.Id,
        title: t.Title,
        description: t.Description,
        status: t.Status,
        AssignedTo: t.AssignedTo,
      }));
      setTasks(formatted);
    } catch (err) {
      console.error(err);
      alert("Failed to load tasks");
    }
  };

  // ---------- FETCH USERS ----------
  const fetchUsers = async () => {
    try {
      const res = await api.get("/User");
      const formatted = res.data.map((u: any) => ({
        id: u.Id,
        name: u.Name,
        role: u.Role,
      }));
      setUsers(formatted);
    } catch (err) {
      console.error(err);
    }
  };

  // ---------- CREATE TASK ----------
  const createTask = async () => {
    if (!title.trim()) return alert("Enter title");
    if (!desc.trim()) return alert("Enter description");
    if (!priority.trim()) return alert("Select priority");
    if (!duedate.trim()) return alert("Select due date");

    try {
      const dto = {
        Id: 0,
        Title: title,
        Description: desc,
        Priority: priority,
        DueDate: duedate,
      };

      await api.post("/TaskItem", dto);
      alert("Task Created!");
      setTitle("");
      setDesc("");
      setPriority("");
      setDuedate("");
      setShowCreateTask(false);
      fetchTasks();
    } catch (err) {
      console.error(err);
      alert("Error creating task");
    }
  };

  // ---------- ASSIGN USER ----------
  const assignUser = async () => {
    if (!selectedUser) return alert("Select a user");
    if (!selectedTask) return;

    try {
      await api.put("/TaskItem/Assign", null, {
        headers: {
          taskId: selectedTask.id.toString(),
          userId: selectedUser.toString(),
        },
      });
      alert("User Assigned!");
      setAssignPopup(false);
      setSelectedUser("");
      fetchTasks();
    } catch (err) {
      console.error(err);
      alert("Failed to assign user");
    }
  };

  return (
    <div className="min-h-screen bg-gray-900 text-gray-100">
      <NavBar name={adminName} />

      <div className="p-6 max-w-6xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Admin Dashboard</h1>

        {/* Toggle Create Task */}
        <button
          onClick={() => setShowCreateTask(!showCreateTask)}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition mb-6"
        >
          {showCreateTask ? "Hide Task Form" : "Add Task"}
        </button>

        {/* CREATE TASK FORM */}
        {showCreateTask && (
          <div className="bg-gray-800 p-6 rounded-md shadow mb-8">
            <h2 className="text-xl font-semibold mb-4">Create Task</h2>

            <input
              type="text"
              placeholder="Task Title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="w-full mb-4 p-2 rounded bg-gray-700 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            />

            <textarea
              placeholder="Task Description"
              value={desc}
              onChange={(e) => setDesc(e.target.value)}
              className="w-full mb-4 p-2 rounded bg-gray-700 text-white h-24 resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
            />

            <select
              value={priority}
              onChange={(e) => setPriority(e.target.value)}
              className="w-full mb-4 p-2 rounded bg-gray-700 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">Select Priority</option>
              <option value="High">High</option>
              <option value="Medium">Medium</option>
              <option value="Low">Low</option>
            </select>

            <input
              type="date"
              value={duedate}
              onChange={(e) => setDuedate(e.target.value)}
              className="w-full mb-4 p-2 rounded bg-gray-700 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            />

            <button
              onClick={createTask}
              className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 transition"
            >
              Create Task
            </button>
          </div>
        )}

        {/* TASK LIST */}
        <div className="overflow-x-auto">
          <table className="w-full table-auto bg-gray-800 rounded-md border border-gray-700">
            <thead className="bg-gray-700 text-left">
              <tr>
                <th className="px-4 py-2">ID</th>
                <th className="px-4 py-2">Title</th>
                <th className="px-4 py-2">Status</th>
                <th className="px-4 py-2">Assigned To</th>
                <th className="px-4 py-2">Assign</th>
              </tr>
            </thead>
            <tbody>
              {tasks.map((task) => (
                <tr key={task.id} className="border-t border-gray-700">
                  <td className="px-4 py-2">{task.id}</td>
                  <td className="px-4 py-2">{task.title}</td>
                  <td className="px-4 py-2">{task.status}</td>
                  <td className="px-4 py-2">
                    {task.AssignedTo
                      ? users.find((u) => u.id === task.AssignedTo)?.name || "Unknown User"
                      : "Not Assigned"}
                  </td>
                  <td className="px-4 py-2">
                    <button
                      onClick={() => {
                        setSelectedTask(task);
                        setAssignPopup(true);
                      }}
                      className="px-2 py-1 bg-purple-600 rounded hover:bg-purple-700 transition"
                    >
                      Assign User
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* ASSIGN USER POPUP */}
        {assignPopup && selectedTask && (
          <div className="fixed inset-0 bg-black bg-opacity-60 flex justify-center items-center">
            <div className="bg-gray-800 p-6 rounded-md w-80">
              <h3 className="text-lg font-semibold mb-4">
                Assign User → Task #{selectedTask.id}
              </h3>

              <select
                value={selectedUser}
                onChange={(e) => setSelectedUser(e.target.value)}
                className="w-full mb-4 p-2 rounded bg-gray-700 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="">Select User</option>
                {users.map((u) => (
                  <option key={`user-${u.id}`} value={u.id}>
                    {u.name}
                  </option>
                ))}
              </select>

              <div className="flex justify-end space-x-2">
                <button
                  onClick={assignUser}
                  className="px-3 py-1 bg-green-600 rounded hover:bg-green-700 transition"
                >
                  Assign
                </button>
                <button
                  onClick={() => setAssignPopup(false)}
                  className="px-3 py-1 bg-red-600 rounded hover:bg-red-700 transition"
                >
                  Cancel
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
