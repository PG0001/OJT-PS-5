import { useEffect, useState } from "react";
import api from "../../api/api";
import NavBar from "../../NavBar";

interface Task {
  id: number;
  title: string;
  description: string;
  status: string;
  assignedUserId: number;
  comments: string[];
}

export default function EmployeeDashboard() {
  const [myTasks, setMyTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(false);
  const userId = localStorage.getItem("userId") || "";

  const loadTasks = async () => {
    if (!userId) return;
    setLoading(true);

    try {
      const res = await api.get(`/TaskItem/AssignedId?id=${userId}`);
      const tasks = Array.isArray(res.data) ? res.data : [res.data];

      const tasksWithComments = await Promise.all(
        tasks.map(async (t: any) => {
          let comments: string[] = [];
          try {
            const commentRes = await api.get(`/TaskComment/${t.Id}`);
            comments = commentRes.data.map((c: any) => c.CommentText);
          } catch (err: any) {
            if (err.response && err.response.status === 404) {
              comments = [];
            } else {
              console.error("Error fetching comments for task", t.Id, err);
              comments = [];
            }
          }

          return {
            id: t.Id,
            title: t.Title,
            description: t.Description,
            status: t.Status,
            assignedUserId: t.AssignedTo,
            comments,
          };
        })
      );

      setMyTasks(tasksWithComments);
    } catch (err) {
      console.error("Error loading tasks", err);
      alert("Failed to load tasks");
    }

    setLoading(false);
  };

  const handleUpdateStatus = async (taskId: number, status: string) => {
    try {
      await api.put(`/TaskItem/Status`, null, {
        headers: { taskId, status },
      });
      loadTasks();
      alert("Task status updated!");
    } catch (err) {
      console.error("Error updating status", err);
      alert("Failed to update status");
    }
  };

  const handleAddComment = async (taskId: number, text: string) => {
    if (!text) return;
    try {
      await api.post(
        `/TaskComment?taskId=${taskId}&userId=${userId}&text=${encodeURIComponent(text)}`
      );
      loadTasks();
      alert("Comment added!");
    } catch (err) {
      console.error("Error adding comment", err);
      alert("Failed to add comment");
    }
  };

  useEffect(() => {
    loadTasks();
  }, []);

  return (
    <div className="min-h-screen bg-gray-900 text-gray-100">
      <NavBar name={localStorage.getItem("name") || ""} />

      <div className="p-6 max-w-4xl mx-auto">
        <h2 className="text-3xl font-semibold mb-4">Employee Dashboard</h2>
        <h3 className="text-xl font-medium mb-4">My Tasks</h3>

        {loading && <p className="text-gray-400">Loading tasks...</p>}
        {!loading && myTasks.length === 0 && <p className="text-gray-400">No tasks assigned.</p>}

        {myTasks.map((task) => (
          <div
            key={task.id}
            className="bg-gray-800 p-4 rounded-md shadow mb-4 border border-gray-700"
          >
            <h4 className="text-lg font-semibold text-white">{task.title}</h4>
            <p className="text-gray-300">{task.description}</p>

            <p className="mt-2">
              <strong>Status:</strong>{" "}
              {task.status === "Completed" ? (
                <span className="text-green-500 font-bold">{task.status}</span>
              ) : (
                <span className="text-gray-200">{task.status}</span>
              )}
            </p>

            <div className="mt-3 flex space-x-2">
              {task.status === "Pending" && (
                <button
                  onClick={() => handleUpdateStatus(task.id, "InProgress")}
                  className="px-3 py-1 bg-green-600 text-white rounded hover:bg-green-700 transition"
                >
                  Start Task
                </button>
              )}

              {task.status === "InProgress" && (
                <button
                  onClick={() => handleUpdateStatus(task.id, "Completed")}
                  className="px-3 py-1 bg-green-600 text-white rounded hover:bg-green-700 transition"
                >
                  Mark Completed
                </button>
              )}

              {task.status === "Completed" && (
                <span className="text-green-500 font-bold">Task Completed</span>
              )}
            </div>

            <div className="mt-4">
              <strong>Comments:</strong>
              <ul className="list-disc list-inside text-gray-300">
                {task.comments.map((c, idx) => (
                  <li key={idx}>{c}</li>
                ))}
              </ul>

              <textarea
                id={`comment-${task.id}`}
                placeholder="Add a comment..."
                className="w-full mt-2 p-2 border border-gray-600 rounded bg-gray-700 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <button
                onClick={() => {
                  const textarea = document.getElementById(
                    `comment-${task.id}`
                  ) as HTMLTextAreaElement;
                  const text = textarea?.value.trim();
                  handleAddComment(task.id, text);
                  if (textarea) textarea.value = "";
                }}
                className="mt-2 px-3 py-1 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
              >
                Add Comment
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
