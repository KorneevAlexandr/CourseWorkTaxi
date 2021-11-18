using System;
using System.Collections.Generic;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Models.Calls
{
	public class CallCollectionViewModel
	{
		public int DriverId { get; set; }

		public int DispatherId { get; set; }

		public DateTime? Date { get; set; }

		public string DateStringFormat
		{
			get
			{
				if (Date == null)
				{
					Date = DateTime.Now;
				}
				var year = Date.Value.Year;
				var month = Date.Value.Month;
				var day = Date.Value.Day;
				return $"{year}-{month}-{day}";
			}
		}

		public int TariffId { get; set; }

		public int CountPages { get; set; }

		public int CurrentPage { get; set; }

		public List<TariffViewModel> Tariffs { get; set; }

		public List<EmployeesSelectViewModel> Drivers { get; set; }

		public List<EmployeesSelectViewModel> Dispathers { get; set; }

		public List<CallViewModel> Calls { get; set; }
	}
}
