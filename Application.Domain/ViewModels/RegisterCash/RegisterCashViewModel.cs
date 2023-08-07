namespace Core.Application.ViewModels.RegisterCash
{
    public class RegisterCashViewModel
    {
        public string CashierId { get; set; }
        public string CashierName { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastConnection { get; set; }


    }
}
