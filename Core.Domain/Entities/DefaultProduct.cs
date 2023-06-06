using Core.Domain.Commons;

namespace Core.Domain.Entities
{
    public class DefaultProduct : CommonsProperty
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public string BarCode { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }

        //Navegation Property
        public Category Category { get; set; }
        public ICollection<Product> Products { get; set;}


    }
}
