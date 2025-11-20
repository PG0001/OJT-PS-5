using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventTrackerAPI.Services.Intefaces;

namespace EventTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = _userService.GetAllUsers();
               if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found.");
                    return NotFound("No users found.");
                }
               _logger.LogInformation("Users retrieved successfully.");
                return Ok(users);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("ID")]
        public IActionResult GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid user ID provided: {Id}", id);
                    return BadRequest("Invalid user ID.");
                }
                var user = _userService.GetUser(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {Id}", id);
                    return NotFound("User not found.");
                }
                _logger.LogInformation("User retrieved successfully with ID: {Id}", id);
                return Ok(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] EventTrackerAPI.Models.User user)
        {
            try
            {
                _logger.LogInformation("Creating a new user.");
                var createdUser = _userService.CreateUser(user);
                if (createdUser != null)
                {
                    _logger.LogInformation("User created successfully with ID: {Id}", createdUser.Id);
                    return Ok(createdUser);
                }
                else
                {
                    _logger.LogError("User creation failed.");
                    return BadRequest("User was not created.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
