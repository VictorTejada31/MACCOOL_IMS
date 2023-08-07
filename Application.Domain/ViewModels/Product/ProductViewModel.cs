using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public string BarCode { get; set; }
        public string Category { get; set; }
        public double SalePrice { get; set; }
        public double PurchasePrice { get; set; }
        public int Amount { get; set; }
        public int DefaultProudctId { get; set; }
        public string CreatedBy { get; set; }
    }
}
