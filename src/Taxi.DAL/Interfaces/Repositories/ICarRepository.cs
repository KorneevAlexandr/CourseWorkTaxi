using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface ICarRepository : IRepository<Car>
	{
		IQueryable<Car> GetAllByQuery(string query, params object[] parameters);
		Task<IQueryable<Car>> GetAllAsync(int skip, int take);
		Task<int> GetCountAsync();
	}
}
