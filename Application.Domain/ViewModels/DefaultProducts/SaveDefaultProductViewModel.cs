using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.DefaultProducts
{
    public class SaveDefaultProductViewModel
    {
        [Required(ErrorMessage = "Campo Requerido.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        //public IFormFile ? Img { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]
        [DataType(DataType.Text)]
        public string BarCode { get; set; }
        public int CategoryId { get; set; }
    }
}
