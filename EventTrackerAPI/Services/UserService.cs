using EventTrackerAPI.Services.Intefaces;
using EventTrackerLibrary;
using EventTrackerAPI.Models;
using EventTrackerLibrary.Models;

namespace EventTrackerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly EventTrackerRepository _repo;

        public UserService(EventTrackerRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
          IEnumerable<EventTrackerLibrary.Models. User> users=  _repo.GetAllUsers();
            List<Models.User> apiUser = new List<Models.User>();
            foreach (var user in users)
            {
               apiUser.Add(new Models.User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Role = user.Role
                });
            }
          return apiUser;
        }

        public Models.User? GetUser(int id)
        {
            try
            {
               EventTrackerLibrary.Models.User users = _repo.GetUserById(id);
                Models.User apiUser = new Models.User()
                {
                    Id = users.Id,
                    Name = users.Name,
                    Email = users.Email,
                    PasswordHash = users.PasswordHash,
                    Role = users.Role

                };
                return apiUser;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Models.User CreateUser(Models.User user)
        {
            EventTrackerLibrary.Models.User newUser = new EventTrackerLibrary.Models.User()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };



            _repo.AddUser(newUser);
            return user;
        }
    }
}
