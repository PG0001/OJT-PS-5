using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EventTrackerAPI.Services.Intefaces;

namespace EventTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                _logger.LogInformation("Attempting login for email: {Email}", email);

                // Validate user credentials
                var user = _authService.Login(email, password);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for email: {Email}", email);
                    return Unauthorized("Invalid email or password.");
                }

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // cookie persists after browser closes
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                // Sign in user and issue cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                _logger.LogInformation("Login successful for email: {Email}", email);

                return Ok(new { Message = "Login successful", User = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for email: {Email}", email);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("Check")]
        public IActionResult Check()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                // Return minimal user info
                return Ok(new { name = User.Identity.Name });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
