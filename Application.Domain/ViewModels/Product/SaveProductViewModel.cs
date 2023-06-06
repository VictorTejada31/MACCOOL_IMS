using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        [Required(ErrorMessage = "Campo Requerido.")]
        [DataType(DataType.Currency)]
        public double SalePrice { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]
        [DataType(DataType.Currency)]
        public double PurchasePrice { get; set; }
        public int DefaultProductId { get; set; }

        [Required(ErrorMessage = "Campo Requerido.")]
        public int Amount { get; set; }
    }
}
