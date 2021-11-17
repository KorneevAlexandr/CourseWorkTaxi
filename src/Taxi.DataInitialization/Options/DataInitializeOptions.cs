using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.DataInitialization.Options
{
	internal static class DataInitializeOptions
	{
		internal static bool Initialize { get; set; }

		internal static int CountCars { get; set; }

		internal static int CountCalls { get; set; }

		internal static int CountEmployees { get; set; }
	}
}
