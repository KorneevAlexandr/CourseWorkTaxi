using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Taxi.UI.Initializers;

namespace Taxi.UI.Diagnostics
{
	public class DefaultDataInializeMiddleware
	{
		private readonly RequestDelegate _next;

		public DefaultDataInializeMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var initializers = context.RequestServices.GetServices<IDefaultInitializer>();
			foreach (var initializer in initializers)
			{
				await initializer.InitializeAsync();
			}

			await _next.Invoke(context);
		}
	}

	public static class DefaultDataInializeMiddlewareExtensions
	{
		public static IApplicationBuilder UseDefaultDataInitilize(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<DefaultDataInializeMiddleware>();
		}
	}
}
