using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class EmployeeInitializer : IInitializer
	{
		private readonly TaxiContext _context;
		private readonly int _count;

		public EmployeeInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountEmployees;
		}

		public bool Initialize { get => DataInitializeOptions.Initialize; }

		public async Task InitializeAsync()
		{
			int x = _count;
			await Task.CompletedTask;
		}
	}
}
