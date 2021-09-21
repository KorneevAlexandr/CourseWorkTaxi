using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Tariffs")]
	public class Tariff
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(40)]
		public string Name { get; set; }

		[Required]
		[MaxLength(200)]
		public string Description { get; set; }

		[Required]
		public double Price { get; set; }
	}
}
