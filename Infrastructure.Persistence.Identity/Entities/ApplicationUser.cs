using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CretedBy { get; set; }
        public bool isOnline { get; set; }
        public string Plan { get; set; }
        public DateTime LastConnection { get; set; }

    }
}
