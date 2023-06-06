
using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Client
{
    public class SaveClientViewModel
    {
        [Required(ErrorMessage = "Debe insertar un nombre.")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [DataType(DataType.Text)]
        public string Tel { get; set; }

        [DataType(DataType.Currency)]
        public string Owed { get; set; }

        [DataType(DataType.Text)]
        public string IdCard { get; set; }
    }
}
