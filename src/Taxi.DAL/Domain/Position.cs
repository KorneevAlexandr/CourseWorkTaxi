using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Positions")]
	public class Position
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(40)]
		public string Name { get; set; }

		[MaxLength(100)]
		public string Description { get; set; }
	}
}
