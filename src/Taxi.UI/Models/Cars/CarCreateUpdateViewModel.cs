using System;
using System.Collections.Generic;
using Taxi.UI.Models.CarModels;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Models.Cars
{
	public class CarCreateUpdateViewModel
	{
		public int Id { get; set; }

		public string RegistrationNumber { get; set; }

		public int BodyNumber { get; set; }

		public int EngineNumber { get; set; }

		public int IssueYear { get; set; }

		public int Mileage { get; set; }

		public DateTime LastTI { get; set; }

		public string BrandName { get; set; }

		public int ModelId { get; set; }

		public int TariffId { get; set; }

		public int DriverId { get; set; }

		public int MechanicId { get; set; }
		
		public List<ModelSelectViewModel> Models { get; set; }

		public List<TariffViewModel> Tariffs { get; set; }

		public List<EmployeesSelectViewModel> Drivers { get; set; }

		public List<EmployeesSelectViewModel> Mechanics { get; set; }
	}
}
