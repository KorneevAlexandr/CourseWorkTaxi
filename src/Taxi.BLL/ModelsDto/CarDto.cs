using System;

namespace Taxi.BLL.ModelsDto
{
	public class CarDto
	{
		public int Id { get; set; }

		public string RegistrationNumber { get; set; }

		public int BodyNumber { get; set; }

		public int EngineNumber { get; set; }

		public int IssueYear { get; set; }

		public int Mileage { get; set; }

		public DateTime LastTI { get; set; }

		public int ModelId { get; set; }

		public int TariffId { get; set; }

		public int DriverId { get; set; }

		public int MechanicId { get; set; }
	}
}
