using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Data
{
	public class User : IdentityUser
	{
		public int EmployeeId { get; set; }
	}
}
