using System.ComponentModel.DataAnnotations;

namespace Taxi.UI.Models.Brands
{
	public class BrandViewModel
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
	}
}
