using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Users
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "*Obligado*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Obligado*")]
        [Compare(nameof(Password),ErrorMessage = "Las contraseñas no coinciden.")]
        public string PasswordConfirm { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }

    }
}
