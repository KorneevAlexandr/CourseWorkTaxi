using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<IQueryable<Employee>> GetAllByPosition(int positionId);
		Task<IQueryable<Employee>> GetAllAsync(int skip, int take);
		Task<int> GetCountAsync();
	}
}
