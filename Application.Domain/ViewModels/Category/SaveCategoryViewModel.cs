using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Category
{
    public class SaveCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*Debe insertar un nombre.*")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string ? Description { get; set; }

        public int ? ModelState { get; set; }

    }
}
