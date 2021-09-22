using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IAccountRepository
	{
		Task<Account> GetAsync(string login);

		Task<Account> GetByEmployee(int id);

		Task CreateAsync(Account entity);

		Task DeleteAsync(int id);

		Task UpdateAsync(Account entity);
	}
}
