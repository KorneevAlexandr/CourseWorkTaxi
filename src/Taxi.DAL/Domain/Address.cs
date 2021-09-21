using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxi.DAL.Domain
{
	[Table("Addresses")]
	public class Address
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string District { get; set; }

		[Required]
		[MaxLength(40)]
		public string Street { get; set; }

		[Required]
		public int HomeNumber { get; set; }
	}
}
