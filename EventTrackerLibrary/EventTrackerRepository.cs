using System;
using System.Collections.Generic;
using System.Linq;
using EventTrackerLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EventTrackerLibrary
{
    public class EventTrackerRepository
    {
        private readonly TrackerContext _context;

        public EventTrackerRepository()
        {
            _context = new TrackerContext();
        }

        // ===========================
        // TASK METHODS
        // ===========================
        #region Task Methods
        public IEnumerable<TaskItem> GetAllTasks()
        {
            try
            {
                return _context.TaskItems.ToList();
            }
            catch (Exception)
            {
                return new List<TaskItem>();
            }
        }

        public TaskItem? GetTaskById(int id)
        {
            try
            {
                return _context.TaskItems.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddTask(TaskItem task)
        {
            try
            {
                _context.TaskItems.Add(task);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTask(TaskItem task)
        {
            try
            {
                _context.TaskItems.Update(task);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTask(int id)
        {
            try
            {
                var task = _context.TaskItems.Find(id);
                if (task == null) return false;

                _context.TaskItems.Remove(task);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AssignTaskToUser(int taskId, int userId)
        {
            try
            {
                var task = _context.TaskItems.Find(taskId);
                if (task == null) return false;

                task.AssignedTo = userId;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<TaskItem> GetTasksByAssignedUser(int assignedUserId)
        {
            try
            {
                return _context.TaskItems
                    .Where(t => t.AssignedTo == assignedUserId)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<TaskItem>();
            }
        }

        #endregion

        // ===========================
        // COMMENTS
        // ===========================
        #region Comment Methods
        public bool AddCommentToTask(int taskId, int userId, string commentText)
        {
            try
            {
                var comment = new TaskComment
                {
                    TaskId = taskId,
                    UserId = userId,
                    CommentText = commentText,
                    CreatedAt = DateTime.Now
                };

                _context.TaskComments.Add(comment);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TaskComment? GetCommentById(int id)
        {
            try
            {
                return _context.TaskComments.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TaskComment> GetAllComments()
        {
            try
            {
                return _context.TaskComments.ToList();
            }
            catch (Exception)
            {
                return new List<TaskComment>();
            }
        }

        public IEnumerable<TaskComment> GetCommentsForTask(int taskId)
        {
            try
            {
                return _context.TaskComments.Where(c => c.TaskId == taskId).ToList();
            }
            catch (Exception)
            {
                return new List<TaskComment>();
            }
        }
        #endregion
        // ===========================
        // USERS
        // ===========================
        #region User Methods
        public User? GetUserById(int id)
        {
            try
            {
                return _context.Users.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        public bool AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
