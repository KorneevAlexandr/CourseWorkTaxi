using Microsoft.AspNetCore.Identity;

namespace Taxi.UI.Data
{
	public class User : IdentityUser
	{
		public int EmployeeId { get; set; }
	}
}
