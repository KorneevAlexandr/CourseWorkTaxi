using System.Threading.Tasks;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IRepository<T>
	{
		Task<T> GetAsync(int id);

		Task CreateAsync(T entity);
		
		Task DeleteAsync(int id);

		Task UpdateAsync(T entity);
	}
}
