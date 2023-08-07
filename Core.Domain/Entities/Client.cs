using Core.Domain.Commons;

namespace Core.Domain.Entities
{
    public class Client : CommonsProperty
    {
        public string FullName { get; set; }
        public string Tel { get; set; }
        public int Owed { get; set; }
        public string IdCard { get; set; }
        public string CreatedBy { get; set; }




    }
}
