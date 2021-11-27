using System;
using System.Collections.Generic;
using Taxi.UI.Models.Positions;

namespace Taxi.UI.Models.Employees
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public DateTime DateStartOfWork { get; set; }

		public int PositionId { get; set; }

		public string PositionName { get; set; }

		public bool Registered { get; set; }

		public string Email { get; set; }

		public List<PositionViewModel> Positions { get; set; }
	}
}
