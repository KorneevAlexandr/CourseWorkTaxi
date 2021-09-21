using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface ICallRepository : IRepository<Call>
	{
		IQueryable<Call> GetAllByQuery(string query, params object[] parameters);
		Task<IQueryable<Call>> GetAllAsync(int skip, int take);
		Task<int> GetCountAsync();
	}
}
