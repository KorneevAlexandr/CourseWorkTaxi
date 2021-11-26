using System.ComponentModel.DataAnnotations;

namespace Taxi.UI.Models.Accounts
{
	public class AccountViewModel
	{
		[Required(ErrorMessage = "Email не должен быть пустым")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Пароль не должен быть пустым")]
		public string Password { get; set; }
	}
}
