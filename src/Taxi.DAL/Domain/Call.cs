using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxi.DAL.Domain
{
	[Table("Calls")]
	public class Call
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public DateTime CallDateTime { get; set; }

		[MaxLength(15)]
		public string Phone { get; set; }
	
		[Required]
		public double Price { get; set; }

		[Required]
		[MaxLength(40)]
		public string StartStreet { get; set; }

		[Required]
		public int StartHomeNumber { get; set; }

		[Required]
		[MaxLength(40)]
		public string EndStreet { get; set; }

		[Required]
		public int EndHomeNumber { get; set; }

		[Required]
		public int CarId { get; set; }

		[ForeignKey("CarId")]
		public Car Car { get; set; }

		[Required]
		public int DispatherId { get; set; }

		[ForeignKey("DispatherId")]
		public Employee Dispather { get; set; }
	}
}
