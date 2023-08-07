
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.DefaultProducts
{
    public class SaveDefaultProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*Campo Requerido.*")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Campo Requerido.*")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "*Campo Requerido.*")]
        [DataType(DataType.Text)]
        public string BarCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "*Seleccione una opción*")]
        public int CategoryId { get; set; }
        public IFormFile ? File { get; set; }
        public string ? Img { get; set; }
        public string ? CreatedBy { get; set; }

        public int ? ModalState { get; set; }
    }
}
