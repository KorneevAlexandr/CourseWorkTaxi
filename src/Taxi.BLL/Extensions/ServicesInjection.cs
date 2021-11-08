using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.Services;
using Taxi.DAL.Extensions;

namespace Taxi.BLL.Extensions
{
	public static class ServicesInjection
	{
		public static IServiceCollection InjectServices(this IServiceCollection services, string connectionString)
		{
			services.InjectRepository(connectionString);

			services.AddScoped<IBrandService, BrandService>();
			services.AddScoped<ICallService, CallService>();
			services.AddScoped<ICarService, CarService>();
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<IPositionService, PositionService>();
			services.AddScoped<IModelService, ModelService>();
			services.AddScoped<ITariffService, TariffService>();

			return services;
		}
	}
}
