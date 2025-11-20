using EventTrackerAPI.Services.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;
        private readonly ITaskService _taskService;
        public TaskItemController(ILogger<TaskItemController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;

        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Fetching all task items.");
                var tasks = _taskService.GetAll();
                if (tasks == null || !tasks.Any())
                {
                    _logger.LogWarning("No task items found.");
                    return NotFound("No task items found.");
                }
                _logger.LogInformation("Task items retrieved successfully.");
                return Ok(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("AssignedId")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.LogInformation("Fetching task item with ID: {Id}", id);
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid task item ID provided: {Id}", id);
                    return BadRequest("Invalid task item ID.");
                }
                var task = _taskService.GetByAssignedUserId(id);
                if (task == null)
                {
                    _logger.LogWarning("Task item not found with ID: {Id}", id);
                    return NotFound("Task item not found.");
                }
                _logger.LogInformation("Task item retrieved successfully with ID: {Id}", id);
                return Ok(task);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] EventTrackerAPI.Models.Dtos.TaskDto dto, [FromHeader] int adminId)
        {
            try
            {
                _logger.LogInformation("Creating a new task item.");
                if (dto == null)
                {
                    _logger.LogWarning("Task item data is null.");
                    return BadRequest("Task item data cannot be null.");
                }
                var result = _taskService.Create(dto, adminId);
                if (!result)
                {
                    _logger.LogError("Failed to create task item.");
                    return BadRequest( "Failed at Creating Task.");
                }
                _logger.LogInformation("Task item created successfully.");
                return Ok("Task item created successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] EventTrackerAPI.Models.Dtos.TaskDto dto, [FromHeader] int adminId)
        {
            try
            {
                _logger.LogInformation("Updating task item.");
                if (dto == null)
                {
                    _logger.LogWarning("Task item data is null.");
                    return BadRequest("Task item data cannot be null.");
                }
                var result = _taskService.Update(adminId, dto);
                if (!result)
                {
                    _logger.LogError("Failed to update task item.");
                    return StatusCode(500, "An error occurred while updating the task item.");
                }
                _logger.LogInformation("Task item updated successfully.");
                return Ok("Task item updated successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("Assign")]
        public IActionResult Assign([FromHeader] int taskId, [FromHeader] int userId)
        {
            try
            {
                _logger.LogInformation("Assigning task item.");
                var result = _taskService.Assign(taskId, userId);
                if (!result)
                {
                    _logger.LogError("Failed to assign task item.");
                    return StatusCode(500, "An error occurred while assigning the task item.");
                }
                _logger.LogInformation("Task item assigned successfully.");
                return Ok("Task item assigned successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("Status")]
        public IActionResult UpdateStatus([FromHeader] int taskId, [FromHeader] string status)
        {
            try
            {
                _logger.LogInformation("Updating task item status.");
                var result = _taskService.UpdateStatus(taskId, status);
                if (!result)
                {
                    _logger.LogError("Failed to update task item status.");
                    return StatusCode(500, "An error occurred while updating the task item status.");
                }
                _logger.LogInformation("Task item status updated successfully.");
                return Ok("Task item status updated successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }   
        }
}
