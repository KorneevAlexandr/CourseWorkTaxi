using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Taxi.DataInitialization.Interfaces;

namespace Taxi.UI.Diagnostics
{
	public class DataInitializeMiddleware
	{
		private readonly RequestDelegate _next;
		private bool _initialize;

		public DataInitializeMiddleware(RequestDelegate next)
		{
			this._next = next;
			_initialize = true;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (_initialize)
			{
				var initializers = context.RequestServices.GetServices<IInitializer>();
				foreach (var initializer in initializers)
				{
					if (initializer.Initialize)
					{
						await initializer.InitializeAsync();
					}
				}
			}

			_initialize = false;

			await _next.Invoke(context);
		}
	}

	public static class DataInitializeMiddlewareExtensions
	{
		public static IApplicationBuilder UseDataInitialize(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<DataInitializeMiddleware>();
		}
	}
}
