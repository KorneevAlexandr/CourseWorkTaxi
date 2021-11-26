using System.ComponentModel.DataAnnotations;

namespace Taxi.UI.Models.Accounts
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Email не должен быть пустым")]
		[EmailAddress(ErrorMessage = "Email не действительный")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Пароль не должен быть пустым")]
		[MinLength(5, ErrorMessage = "Пароль должен быть не менее 5 символов")]
		public string Password { get; set; }

		public int EmployeeId { get; set; }

		public string DataEmployee { get; set; }
	}
}
