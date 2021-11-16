using System;

namespace Taxi.BLL.Exceptions
{
	public class InvalidDeleteOperationException : InvalidOperationException
	{
		private readonly string _nameValue;

		public InvalidDeleteOperationException(string message)
			: base(message)
		{ 
		}

		public InvalidDeleteOperationException(string message, string nameValue)
			: base(message)
		{
			_nameValue = nameValue;
		}

		public string NameValue { get => _nameValue; }
	}
}
