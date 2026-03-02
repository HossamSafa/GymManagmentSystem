using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.AnalyticsViewModel
{
	public class AnalyticsViewModel
	{
		public int TotalMember {  get; set; }
		public int ActiveMember { get; set; }

		public int TotalTrainer { get; set; }

		public int UpcomingSession { get; set; }

		public int OnGoingSession { get; set; }
		public int CompleteSession { get; set; }
	}
}
