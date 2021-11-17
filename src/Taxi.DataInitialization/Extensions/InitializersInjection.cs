using Microsoft.Extensions.DependencyInjection;
using System;
using Taxi.DataInitialization.Initializers;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Extensions
{
	public static class InitializersInjection
	{
		public static IServiceCollection InjectInitializers(this IServiceCollection services, InitializeOptions options)
		{
			DataInitializeOptions.Initialize = options.Initialize;
			DataInitializeOptions.CountCars = options.CountCars;
			DataInitializeOptions.CountCalls = options.CountCalls;
			DataInitializeOptions.CountEmployees = options.CountEmployees;

			services.AddScoped<IInitializer, CarInitializer>();
			services.AddScoped<IInitializer, CallInitializer>();
			services.AddScoped<IInitializer, EmployeeInitializer>();

			return services;
		}
	}
}
