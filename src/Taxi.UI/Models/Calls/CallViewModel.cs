using System;

namespace Taxi.UI.Models.Calls
{
	public class CallViewModel
	{
		public int Id { get; set; }

		public DateTime CallDateTime { get; set; }

		public string Phone { get; set; }

		public double Price { get; set; }

		public string StartStreet { get; set; }

		public int StartHomeNumber { get; set; }

		public string EndStreet { get; set; }

		public int EndHomeNumber { get; set; }

		public string DriverFullName { get; set; }

		public string DispatherFullName { get; set; }
	}
}
