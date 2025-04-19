using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Domain.Users
{
    public class TFGCUser
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public string ProfilePictureUrl { get; set; }
        public bool IsDeleted { get; set; }

        private TFGCUser() { }

        public TFGCUser(string fullname, string email, string passwordHash, string role = "User")
        {
            Id = Guid.NewGuid();
            Fullname = fullname;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            ProfilePictureUrl = "";
        }

        public static TFGCUser Register(string fullname, string email, string plainPassword, string role = "User")
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            var user = new TFGCUser(fullname, email, hashed, role);
            return user;
        }

        public void UpdateProfilePicture(string profilePictureUrl)
        {
            ProfilePictureUrl = profilePictureUrl;
        }

        public void UpdateFullname(string fullname)
        {
            Fullname = fullname;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}