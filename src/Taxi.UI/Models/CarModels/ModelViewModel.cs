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

		public string Body { get; set; }

		public int BodyId { get; set; }

		public List<BodyViewModel> Bodys { get; set; }

		public string Fuel { get; set; }

		public int FuelId { get; set; }

		public List<FuelViewModel> Fuels { get; set; }

		[Required]
		public int HP { get; set; }

		[Required]
		public int Price { get; set; }

		public int BrandId { get; set; }

		public List<BrandViewModel> Brands { get; set; }
	}
}
