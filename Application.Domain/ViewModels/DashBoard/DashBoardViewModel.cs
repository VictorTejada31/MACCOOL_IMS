
using Core.Application.ViewModels.Product;

namespace Core.Application.ViewModels.DashBoard
{
    public class DashBoardViewModel
    {
        public string UserId { get; set; }
        public double ThisMonth { get; set; }
        public double ThisMonthPorcent { get; set; }
        public double Today { get; set; }
        public double TodayPorcent { get; set; }
        public int Products { get; set; }
        public List<ProductViewModel> TopProducts { get; set; }
        public List<ProductViewModel> AlertProduct { get; set; }

    }
}
