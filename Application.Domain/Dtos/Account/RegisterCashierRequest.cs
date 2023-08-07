namespace Core.Application.Dtos.Account
{
    public class RegisterCashierRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Password { get; set; }
    }
}
