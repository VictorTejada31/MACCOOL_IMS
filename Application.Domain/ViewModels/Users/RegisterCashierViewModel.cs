using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Users
{
    public class RegisterCashierViewModel
    {

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "*Obligado*")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "*Obligado*")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "*Obligado*")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "*Obligado*")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*Obligado*")]
        public string Tel { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*Obligado*")]
        public string Password { get; set; }

    }
}
