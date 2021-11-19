using Microsoft.EntityFrameworkCore;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Data
{
	public class TaxiContext : DbContext
	{
		public TaxiContext(DbContextOptions<TaxiContext> options)
			: base(options)
		{
		}

		public DbSet<Brand> Brands { get; set; }

		public DbSet<Call> Calls { get; set; }

		public DbSet<Car> Cars { get; set; }

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Model> Models { get; set; }

		public DbSet<Position> Positions { get; set; }
		
		public DbSet<Tariff> Tariffs { get; set; }
	}
}
