using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Users
{
    public class RegisterFreeViewModel
    {
        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "La contraseña no coincide")]
        public string ConfirmPassword { get; set; }
        public string ? Plan { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
