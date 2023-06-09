

using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "*Obligado*")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*Obligado*")]
        public string Password { get; set; }
        public bool HasError { get; set; }
        public string ? Error { get; set; }
    }
}
