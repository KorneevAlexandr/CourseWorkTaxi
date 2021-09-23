using System;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<IQueryable<Employee>> GetAllAsync(Func<IQueryable<Employee>, IQueryable<Employee>> func);
		Task<IQueryable<Employee>> GetAllAsync(int skip, int take);
		Task<int> GetCountAsync(Func<IQueryable<Employee>, IQueryable<Employee>> predicate);
		Task<Employee> GetLastAsync();
	}
}
