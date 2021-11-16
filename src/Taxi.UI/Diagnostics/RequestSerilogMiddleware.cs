using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Taxi.UI.Diagnostics
{
	public class RequestSerilogMiddleware
	{
        private readonly RequestDelegate _next;

        public RequestSerilogMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
			var request = context.Request;

			var userName = context.User.Identity.Name == string.Empty ? "Anonymous" : context.User.Identity.Name;
			var claimRole = ((ClaimsIdentity)context.User.Identity).Claims
				.FirstOrDefault(x => x.Type == ClaimTypes.Role);
			var userRole = claimRole == null ? "Anonymous" : claimRole.Value;

			var logTemplate = string.Concat("Path: ", request.Host, request.Path, "; ",
										  "Protocol: ", request.Protocol, "; ",
										  "Method: ", request.Method, "; ",
										  "User(email - role): ", context.User.Identity.Name, " - ",
										  userRole, ".");

			Log.Information(logTemplate);
            await _next.Invoke(context);
        }
    }

	public static class RequestSerilogMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestSerilog(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RequestSerilogMiddleware>();
		}
	}
}
