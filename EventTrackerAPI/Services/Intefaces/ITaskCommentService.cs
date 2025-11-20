using EventTrackerLibrary.Models;

namespace EventTrackerAPI.Services.Intefaces
{
    public interface ITaskCommentService
    {
        IEnumerable<TaskComment> GetComments(int taskId);
        TaskComment AddComment(int taskId, int userId, string text);
    }


}
