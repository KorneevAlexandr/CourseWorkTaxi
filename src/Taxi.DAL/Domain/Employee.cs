using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Employees")]
	public class Employee
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(30)]
		public string Name { get; set; }

		[Required]
		[MaxLength(30)]
		public string Surname { get; set; }

		[Required]
		public DateTime DateStartOfWork { get; set; }

		[Required]
		public int PositionId { get; set; }

		[ForeignKey("PositionId")]
		public Position Position { get; set; }
	}
}
