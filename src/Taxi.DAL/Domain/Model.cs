using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Models")]
	public class Model
	{
		[Key]
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

		[Required]
		public int BrandId { get; set; }

		[ForeignKey("BrandId")]
		public Brand Brand { get; set; }
	}
}
