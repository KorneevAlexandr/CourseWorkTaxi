using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Models.Accounts
{
	public class RegisterViewModel
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public int EmployeeId { get; set; }

		public string DataEmployee { get; set; }
	}
}
