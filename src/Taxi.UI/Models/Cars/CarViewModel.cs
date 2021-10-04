using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.Cars
{
	public class CarViewModel
	{
		public int Id { get; set; }

		public string RegistrationNumber { get; set; }

		public int BodyNumber { get; set; }

		public int EngineNumber { get; set; }

		public int IssueYear { get; set; }

		public int Mileage { get; set; }

		public DateTime LastTI { get; set; }

		public string ModelName { get; set; }

		public string DriverFullName { get; set; }

		public string MechanicFullName { get; set; }

		public string TariffName { get; set; }

		public int Price { get; set; }
	}
}
