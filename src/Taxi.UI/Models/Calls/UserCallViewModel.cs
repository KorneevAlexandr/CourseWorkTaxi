using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Models.Calls
{
	public class UserCallViewModel
	{	
		public int TariffId { get; set; }

		public List<TariffViewModel> Tariffs { get; set; }

		[Required(ErrorMessage = "Укажите номер телефона")]
		[Phone(ErrorMessage = "Номер телефона некорректный")]
		public string Phone { get; set; }

		public string StartStreet { get; set; }

		public int StartHomeNumber { get; set; }

		public string EndStreet { get; set; }

		public int EndHomeNumber { get; set; }
	}
}
