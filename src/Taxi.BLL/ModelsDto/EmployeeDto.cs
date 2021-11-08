using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.BLL.ModelsDto
{
	public class EmployeeDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public DateTime DateStartOfWork { get; set; }

		public int PositionId { get; set; }

		public string PositionName { get; set; }

	}
}
