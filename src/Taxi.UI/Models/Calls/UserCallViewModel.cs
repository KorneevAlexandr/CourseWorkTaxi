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

		[Required(ErrorMessage = "Укажите начальную улицу")]
		[MaxLength(40, ErrorMessage = "Название начальной улицы слишком длинное")]
		public string StartStreet { get; set; }

		[Required(ErrorMessage = "Укажите номер начального дома")]
		[Range(1, 1000, ErrorMessage = "Номер начального дома может быть от 1 до 1000")]
		public int StartHomeNumber { get; set; }

		[Required(ErrorMessage = "Укажите конечную улицу")]
		[MaxLength(40, ErrorMessage = "Название конечной улицы слишком длинное")]
		public string EndStreet { get; set; }

		[Required(ErrorMessage = "Укажите номер конечного дома")]
		[Range(1, 1000, ErrorMessage = "Номер конечного дома может быть от 1 до 1000")]
		public int EndHomeNumber { get; set; }
	}
}
