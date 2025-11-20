using EventTrackerAPI.Services.Intefaces;
using EventTrackerLibrary;
using EventTrackerLibrary.Models;
using System;

namespace EventTrackerAPI.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly EventTrackerRepository _repo;

        public TaskCommentService(EventTrackerRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<TaskComment> GetComments(int taskId)
        {
            return _repo.GetCommentsForTask(taskId);
        }

        public TaskComment AddComment(int taskId, int userId, string text)
        {
            _repo.AddCommentToTask(taskId, userId, text);
            return new TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                CommentText = text,
                CreatedAt = DateTime.Now
            };
        }
    }


}
