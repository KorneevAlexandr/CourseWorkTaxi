using System.Linq;
using System.Threading.Tasks;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface ICompleteRepository<T> : IRepository<T>
	{
		Task<IQueryable<T>> GetAllAsync();
	}
}
