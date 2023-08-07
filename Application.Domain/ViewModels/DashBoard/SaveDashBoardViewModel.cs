
namespace Core.Application.ViewModels.DashBoard
{
    public class SaveDashBoardViewModel
    {
        public string UserId { get; set; }

        public double ThisMonth { get; set; }
        public double Today { get; set; }
        public double LastMonth { get; set; }
        public double Yesterday { get; set; }
        public DateTime TodayDate { get; set; }
        public DateTime ThisMonthDate { get; set; }



    }
}
