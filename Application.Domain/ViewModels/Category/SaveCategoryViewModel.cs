using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Category
{
    public class SaveCategoryViewModel
    {
        [Required(ErrorMessage = "Debe insertar un nombre.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}
