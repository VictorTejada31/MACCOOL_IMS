namespace Core.Application.Dtos.Account
{
    public class CashierModelResponse
    {
        public string Name { get; set; }
        public bool isOnline { get; set; }
        public bool isActive { get; set; }
        public string CashierId { get; set; }
    }
}
