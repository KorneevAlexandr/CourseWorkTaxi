using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Taxi.DAL.Domain
{
	[Table("Accounts")]
	public class Account
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Login { get; set; }

		[Required]
		[MaxLength(30)]
		public string Password { get; set; }

		[Required]
		public int EmployeeId { get; set; }

		[ForeignKey("EmployeeId")]
		public Employee Employee { get; set; }
	}
}
