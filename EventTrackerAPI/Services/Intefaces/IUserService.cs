using EventTrackerAPI.Models;

namespace EventTrackerAPI.Services.Intefaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetUser(int id);
        User CreateUser(User user);
    }


}
