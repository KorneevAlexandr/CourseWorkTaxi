using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using Taxi.BLL.Exceptions;

namespace Taxi.UI.Filters
{
	public class OperationExceptionFilter : Attribute, IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			if (!context.ExceptionHandled && context.Exception is InvalidDeleteOperationException)
			{
				var deleteException = ((InvalidDeleteOperationException)(context.Exception));
				context.Result = new RedirectResult("~/Home/DeleteError");
				context.HttpContext.Response.Cookies.Append("DeleteErrorMessage", deleteException.Message);

				var logTemplate = string.Concat("An exception filter was invoked while trying to delete data. Object to remove: '",
					deleteException.NameValue, "'.");
				Log.Warning(logTemplate);

				context.ExceptionHandled = true;
			}
			else if (!context.ExceptionHandled && context.Exception is InvalidCreateOperationException)
			{
				var createException = ((InvalidCreateOperationException)(context.Exception));
				context.Result = new RedirectResult("~/Home/DeleteError");
				context.HttpContext.Response.Cookies.Append("DeleteErrorMessage", createException.Message);

				var logTemplate = string.Concat("An exception filter was invoked while trying to create non valid data.");
				Log.Warning(logTemplate);

				context.ExceptionHandled = true;
			}
			else
			{
				context.Result = new RedirectResult("~/Home/DeleteError");
				context.HttpContext.Response.Cookies.Append("DeleteErrorMessage", context.Exception.Message);

				Log.Warning("An exception filter was invoked while trying to delete data.");

				context.ExceptionHandled = true;
			}
		}
	}
}
