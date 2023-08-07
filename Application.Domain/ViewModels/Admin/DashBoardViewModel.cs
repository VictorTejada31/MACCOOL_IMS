
using Core.Application.ViewModels.Product;

namespace Core.Application.ViewModels.Admin
{
    public class DashBoardViewModel
    {
        public int ThisMonth { get; set; }
        public int ThisMonthPercent { get; set; }
        public int Today { get; set; }
        public int TodayPercent { get; set; }
        public int Products { get; set; }
        public int ProductsPercent { get; set; }
        public List<ProductViewModel> TopProducts { get; set; }  
        public List<ProductViewModel> MustBuyProducts { get; set; }



    }
}
