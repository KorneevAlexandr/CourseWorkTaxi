using System.Threading.Tasks;

namespace Taxi.DataInitialization.Interfaces
{
	public interface IInitializer
	{
		Task InitializeAsync();

		bool Initialize { get; }
	}
}
