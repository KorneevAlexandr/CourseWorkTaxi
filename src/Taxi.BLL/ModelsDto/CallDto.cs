using System;

namespace Taxi.BLL.ModelsDto
{
	public class CallDto
	{
		public int Id { get; set; }

		public string Phone { get; set; }

		public double Price { get; set; }

		public DateTime CallDateTime { get; set; }

		public string StartStreet { get; set; }

		public string EndStreet { get; set; }

		public int StartHomeNumber { get; set; }

		public int EndHomeNumber { get; set; }

		public int CarId { get; set; }

		public int DispatherId { get; set; }

		public int DriverId { get; set; }
	}
}
