using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.Calls
{
	public class SuccessUserCallViewModel
	{
		public UserCallViewModel UserCall { get; set; }

		public string TariffName { get; set; }

		public string DriverFullName { get; set; }

		public string CarName { get; set; }

		public string RegistrationName { get; set; }

		public double Price { get; set; }

		public int CarId { get; set; }

		public int DriverId { get; set; }

		public int DispatherId { get; set; }

		public bool IsSuccessed { get; set; }
	}
}
