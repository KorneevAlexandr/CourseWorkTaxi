using System.Collections.Generic;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Models.Calls
{
	public class UserCallViewModel
	{	
		public int TariffId { get; set; }

		public List<TariffViewModel> Tariffs { get; set; }

		public string Phone { get; set; }

		public string StartStreet { get; set; }

		public int StartHomeNumber { get; set; }

		public string EndStreet { get; set; }

		public int EndHomeNumber { get; set; }
	}
}
