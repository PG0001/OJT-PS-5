using EventTrackerAPI.Models;

namespace EventTrackerAPI.Services.Intefaces
{
    public interface IAuthService
    {
        User? Login(string email, string password);
    }


}
