using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DataInitialization.Interfaces;

namespace Taxi.DataInitialization.Initializers
{
	internal class CallInitializer : IInitializer
	{
		private readonly TaxiContext _context;

		public CallInitializer(TaxiContext context)
		{
			_context = context;
		}

		public async Task InitializeAsync(int count)
		{
			await Task.CompletedTask;
		}
	}
}
