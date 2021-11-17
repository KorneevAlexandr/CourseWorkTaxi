using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class CarInitializer : IInitializer
	{
		private readonly TaxiContext _context;
		private readonly int _count;

		public CarInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountCars;
		}

		public bool Initialize { get => DataInitializeOptions.Initialize; }

		public async Task InitializeAsync()
		{
			if (!Initialize)
			{
				return;
			}
			await Task.CompletedTask;
		}
	}
}
