using System.Collections.Generic;
using Taxi.UI.Models.Positions;

namespace Taxi.UI.Models.Employees
{
	public class EmployeeCollectionViewModel
	{
		public int SelectedPositionId { get; set; }

		public int YearStanding { get; set; }

		public int CountPages { get; set; }

		public int CurrentPage { get; set; }

		public List<PositionViewModel> Positions { get; set; }

		public List<EmployeeViewModel> Employees { get; set; }
	}
}
