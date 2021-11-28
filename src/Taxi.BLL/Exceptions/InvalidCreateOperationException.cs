using System;

namespace Taxi.BLL.Exceptions
{
	public class InvalidCreateOperationException : InvalidOperationException
	{
		public InvalidCreateOperationException(string message)
			: base(message)
		{
		}
	}
}
