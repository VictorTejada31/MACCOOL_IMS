using Core.Application.ViewModels.Category;

namespace Core.Application.ViewModels.DefaultProducts
{
    public class DefaultProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public string BarCode { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public bool isAdded { get; set; }
       
    }
}
