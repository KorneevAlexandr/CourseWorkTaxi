using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.Users
{
	public class UserViewModel
	{
		public string Name { get; set; }

		public string Surname { get; set; }

		public string Email { get; set; }

		public DateTime StartWork { get; set; }

		public string PositionName { get; set; }

		public string RoleName { get; set; }
	}
}
