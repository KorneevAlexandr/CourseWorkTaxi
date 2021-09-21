using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly TaxiContext _context;

		public AccountRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<Account> GetAsync(string login)
		{
			return await _context.Accounts.FirstOrDefaultAsync(x => x.Login.Equals(login));
		}

		public async Task CreateAsync(Account entity)
		{
			await _context.Accounts.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Accounts.FirstOrDefault(item => item.Id == id);
			_context.Accounts.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Account entity)
		{
			var item = await _context.Accounts.FirstOrDefaultAsync(x => x.EmployeeId == entity.EmployeeId);
			item = entity;
			_context.Accounts.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
