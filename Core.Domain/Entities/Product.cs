namespace Core.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public double SalePrice { get; set; }
        public double PurchasePrice { get; set; }
        public int DefaultProductId { get; set; }

        //Navegation Property

        public DefaultProduct DefaultProduct { get; set; }




    }
}
