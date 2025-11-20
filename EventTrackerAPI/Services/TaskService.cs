using EventTrackerAPI.Models.Dtos;
using EventTrackerAPI.Services.Intefaces;
using EventTrackerLibrary;
using EventTrackerAPI.Models;

namespace EventTrackerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly EventTrackerRepository _repo;

        public TaskService(EventTrackerRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            var response = _repo.GetAllTasks();
            var result = new List<EventTrackerAPI.Models.TaskItem>();

            foreach (var t in response)
            {
                result.Add(new EventTrackerAPI.Models.TaskItem
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    AssignedTo = t.AssignedTo,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate
                });
            }

            return result;
        }

        public TaskItem? GetById(int id)
        {
           var response =_repo.GetTaskById(id);
            var result = new TaskItem();
            result.Status = response.Status;
            result.CreatedAt = response.CreatedAt;
            result.DueDate = response.DueDate;
            result.AssignedTo = response.AssignedTo;
            result.Priority = response.Priority;
            result.Description = response.Description;
            result.Title = response.Title;


            return result;
        }
        public List<TaskItem> GetByAssignedUserId(int id)
        {
            var response = _repo.GetTasksByAssignedUser(id);

            // If no tasks, return empty list
            if (response == null || response.Count == 0)
                return new List<TaskItem>();

            // Map tasks (if needed)
            var result = response.Select(t => new TaskItem
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                AssignedTo = t.AssignedTo,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate,
              
            }).ToList();

            return result;
        }


        public bool Create(TaskDto dto, int adminId)
        {
            var model = new EventTrackerLibrary.Models.TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "Pending",
                DueDate = dto.DueDate,
                CreatedAt = DateTime.Now,
            };

            var response =_repo.AddTask(model);

            

            return response;
        }

        public bool Update(int id, TaskDto dto)
        {
            var existing = _repo.GetTaskById(id);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Priority = dto.Priority;
            existing.DueDate = dto.DueDate;

            _repo.UpdateTask(existing);
            return true;
        }

        public bool Assign(int taskId, int userId)
        {
            var task = _repo.GetTaskById(taskId);
            if (task == null) return false;

            _repo.AssignTaskToUser(taskId, userId);
            return true;
        }

        public bool UpdateStatus(int taskId, string status)
        {
            var task = _repo.GetTaskById(taskId);
            if (task == null) return false;

            task.Status = status;
            _repo.UpdateTask(task);
            return true;
        }
    }
}
