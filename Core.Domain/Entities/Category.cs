
namespace Core.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navegation Property

        public ICollection<DefaultProduct> DefaultProducts { get; set; }
    }
}
