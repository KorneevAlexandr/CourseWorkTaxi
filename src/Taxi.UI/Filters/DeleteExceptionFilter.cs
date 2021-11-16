using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

				context.ExceptionHandled = true;
			}
		}
	}
}
