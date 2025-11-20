using EventTrackerAPI.Models.Dtos;
using EventTrackerAPI.Models;

namespace EventTrackerAPI.Services.Intefaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem? GetById(int id);
        bool Create(TaskDto dto, int adminId);
        bool Update(int id, TaskDto dto);
        bool Assign(int taskId, int userId);
        bool UpdateStatus(int taskId, string status);
        public List<TaskItem> GetByAssignedUserId(int id);
    }

}
