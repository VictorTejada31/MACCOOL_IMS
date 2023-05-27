namespace Core.Domain.Entities
{
    public class DefaultProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public string BarCode { get; set; }
        public string CategoryId { get; set; }
        public string CreatedBy { get; set; }

        //Navegation Property
        public Category Category { get; set; }
        public ICollection<Product> Products { get; set;}


    }
}
