using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class CallInitializer : IInitializer
	{
		private readonly TaxiContext _context;
		private readonly int _count;

		public CallInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountCalls;
		}

		public bool Initialize { get => DataInitializeOptions.Initialize; }

		public async Task InitializeAsync()
		{
			var x = _count;
			await Task.CompletedTask;
		}
	}
}
