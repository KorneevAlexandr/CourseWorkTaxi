using System;

namespace Taxi.UI.Models.Users
{
	public class UserViewModel
	{
		public string UserId { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public string Email { get; set; }

		public DateTime StartWork { get; set; }

		public string PositionName { get; set; }

		public string RoleName { get; set; }
	}
}
