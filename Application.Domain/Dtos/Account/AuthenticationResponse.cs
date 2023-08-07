using Core.Application.Dtos.Commons;

namespace Core.Application.Dtos.Account
{
    public class AuthenticationResponse : CommonsProperties
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CreatedBy { get; set; }
        public List<string> Roles { get; set; }

    }
}
