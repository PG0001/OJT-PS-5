using EventTrackerAPI.Services.Intefaces;
using EventTrackerLibrary;
using EventTrackerAPI.Models;
using System;
using System.Text;
using System.Security.Cryptography;
namespace EventTrackerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly EventTrackerRepository _repo;

        public AuthService(EventTrackerRepository repo)
        {
            _repo = repo;
        }

        public User? Login(string email, string password)
        {
            var user = _repo.GetAllUsers()
                            .FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            // verify hashed password
            //var hashed = HashPassword(password);

            //if (user.PasswordHash != hashed)
            //    return null;

            var user1 = new User
            {
                Id = user.Id,
                Name = user.Name,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                Role = user.Role
            };


            return user1;
        }

        //private string HashPassword(string password)
        //{
        //    using var sha = SHA256.Create();
        //    var bytes = Encoding.UTF8.GetBytes(password);
        //    var hash = sha.ComputeHash(bytes);
        //    return Convert.ToHexString(hash);
        //}
    }

}
