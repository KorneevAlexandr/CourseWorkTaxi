using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Taxi.UI.Models.Brands;

namespace Taxi.UI.Models.CarModels
{
	public class ModelViewModel
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }

		[Required]
		[MaxLength(30)]
		public string Body { get; set; }

		[Required]
		[MaxLength(20)]
		public string Fuel { get; set; }

		[Required]
		public int HP { get; set; }

		[Required]
		public int Price { get; set; }

		public int BrandId { get; set; }

		public List<BrandViewModel> Brands { get; set; }
	}
}
