using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using Taxi.BLL.Exceptions;

namespace Taxi.UI.Filters
{
	public class DeleteExceptionFilter : Attribute, IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			if (!context.ExceptionHandled && context.Exception is InvalidDeleteOperationException)
			{
				var deleteException = ((InvalidDeleteOperationException)(context.Exception));
				context.Result = new ContentResult
				{
					Content = deleteException.Message + deleteException.NameValue,
				};

				var logTemplate = string.Concat("An exception filter was invoked while trying to delete data. Object to remove: '",
					deleteException.NameValue, "'.");
				Log.Warning(logTemplate);

				context.ExceptionHandled = true;
			}
			else
			{
				context.Result = new ContentResult
				{
					Content = context.Exception.Message,
				};

				Log.Warning("An exception filter was invoked while trying to delete data.");

				context.ExceptionHandled = true;
			}
		}
	}
}
