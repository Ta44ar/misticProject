using Microsoft.AspNetCore.Identity;

namespace misticProject.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public override string? PasswordHash { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Subscriber,
        RegisteredUser,
        Guest
    }
}
