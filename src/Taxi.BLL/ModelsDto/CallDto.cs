using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.BLL.ModelsDto
{
	public class CallDto
	{
		public int Id { get; set; }

		public string Phone { get; set; }

		public double Price { get; set; }

		public DateTime CallDateTime { get; set; }

		public string StartAddress { get; set; }

		public string EndAddress { get; set; }

		public int CarId { get; set; }

		public int DispatherId { get; set; }

		public int DriverId { get; set; }
	}
}
