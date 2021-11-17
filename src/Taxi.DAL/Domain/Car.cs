using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Cars")]
	public class Car
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(9)]
		public string RegistrationNumber { get; set; }

		[Required]
		public int BodyNumber { get; set; }

		[Required]
		public int EngineNumber { get; set; }

		[Required]
		public int IssueYear { get; set; }

		public int Mileage { get; set; }

		[Required]
		public DateTime LastTI { get; set; }

		[Required]
		public int ModelId { get; set; }

		[ForeignKey("ModelId")]
		public Model Model { get; set; }

		[Required]
		public int TariffId { get; set; }

		[ForeignKey("TariffId")]
		public Tariff Tariff { get; set; }

		[Required]
		public int DriverId { get; set; }
		
		[ForeignKey("DriverId")]
		public Employee Driver { get; set; }

		[Required]
		public int MechanicId { get; set; }

		[ForeignKey("MechanicId")]
		public Employee Mechanic { get; set; }
	}
}
