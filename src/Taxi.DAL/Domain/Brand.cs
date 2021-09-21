using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Brands")]
	public class Brand
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(30)]
		[Required]
		public string Name { get; set; }
	}
}
