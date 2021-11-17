using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DataInitialization.Interfaces;

namespace Taxi.DataInitialization.Initializers
{
	internal class CarInitializer : IInitializer
	{
		private readonly TaxiContext _context;

		public CarInitializer(TaxiContext context)
		{
			_context = context;
		}

		public async Task InitializeAsync(int count)
		{
			await Task.CompletedTask;
		}
	}
}
