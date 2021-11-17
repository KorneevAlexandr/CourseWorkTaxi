using Microsoft.Extensions.DependencyInjection;
using Taxi.DataInitialization.Initializers;
using Taxi.DataInitialization.Interfaces;

namespace Taxi.DataInitialization.Extensions
{
	public static class InitializersInjection
	{
		public static IServiceCollection InjectInitializers(IServiceCollection services)
		{
			services.AddScoped<IInitializer, CarInitializer>();
			services.AddScoped<IInitializer, CallInitializer>();
			services.AddScoped<IInitializer, EmployeeInitializer>();

			return services;
		}
	}
}
