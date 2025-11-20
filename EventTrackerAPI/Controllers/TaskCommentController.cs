using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventTrackerAPI.Services.Intefaces;

namespace EventTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly ILogger<TaskCommentController> _logger;
        private readonly ITaskCommentService _taskCommentService;
        public TaskCommentController(ILogger<TaskCommentController> logger, ITaskCommentService taskCommentService)
        {
            _logger = logger;
            _taskCommentService = taskCommentService;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.LogInformation("Fetching all task comments.");
                var comments = _taskCommentService.GetComments(id);
                if (comments == null || !comments.Any())
                {
                    _logger.LogWarning("No task comments found.");
                    return NotFound("No task comments found.");
                }
                _logger.LogInformation("Task comments retrieved successfully.");
                return Ok(comments);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult Post(int taskId, int userId, string text)
        {
            try
            {
                _logger.LogInformation("Adding a new comment to task ID: {TaskId}", taskId);
                var comment = _taskCommentService.AddComment(taskId, userId, text);
                if (comment == null)
                {
                    _logger.LogWarning("Failed to add comment to task ID: {TaskId}", taskId);
                    return BadRequest("Failed to add comment.");
                }
                else
                {
                    _logger.LogInformation("Comment added successfully to task ID: {TaskId}", taskId);
                    return Ok(comment);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    }
