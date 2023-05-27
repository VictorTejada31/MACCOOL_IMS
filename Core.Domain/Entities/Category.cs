
using Core.Domain.Commons;

namespace Core.Domain.Entities
{
    public class Category : CommonsProperty
    {
        public string Name { get; set; }

        //Navegation Property

        public ICollection<DefaultProduct> DefaultProducts { get; set; }
    }
}
