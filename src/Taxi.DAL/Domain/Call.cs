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
		public int StartAddressId { get; set; }

		[ForeignKey("StartAddressId")]
		public Address StartAddress { get; set; }

		[Required]
		public int EndAddressId { get; set; }

		[ForeignKey("EndAddressId")]
		public Address EndAddress { get; set; }

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
