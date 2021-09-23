using Microsoft.EntityFrameworkCore;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Data
{
	internal class TaxiContext : DbContext
	{
		private readonly string _conntectionString;

		public TaxiContext()
		{
			_conntectionString = "Server = SANCHOZ; Database = Taxi.CourseWorkDB; Trusted_Connection = True;";
		}

		public TaxiContext(string conntectionString)
		{
			_conntectionString = conntectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_conntectionString);
		}

		public DbSet<Account> Accounts { get; set; }

		public DbSet<Brand> Brands { get; set; }

		public DbSet<Call> Calls { get; set; }

		public DbSet<Car> Cars { get; set; }

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Model> Models { get; set; }

		public DbSet<Position> Positions { get; set; }
		
		public DbSet<Tariff> Tariffs { get; set; }
	}
}
