using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido.")]
        public double SalePrice { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]
        public double PurchasePrice { get; set; }
        public int DefaultProductId { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]

        public int Amount { get; set; }

        public string ? UserId { get; set; }
    }
}
