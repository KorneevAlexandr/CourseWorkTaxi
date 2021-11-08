using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.DAL.Extensions
{
	public static class RepositoryInjection
	{
		public static IServiceCollection InjectRepository(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<TaxiContext>(options => options.UseSqlServer(connectionString));

			services.AddScoped<ICompleteRepository<Brand>, BrandRepository>();
			services.AddScoped<ICallRepository, CallRepository>();
			services.AddScoped<ICarRepository, CarRepository>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<IModelRepository, ModelRepository>();
			services.AddScoped<ICompleteRepository<Position>, PositionRepository>();
			services.AddScoped<ICompleteRepository<Tariff>, TariffRepository>();

			return services;
		}
	}
}
