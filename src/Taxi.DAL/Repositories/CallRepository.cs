using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class CallRepository : ICallRepository
	{
		private readonly TaxiContext _context;

		public CallRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public IQueryable<Call> GetAllByQuery(string query, params object[] parameters)
		{
			return _context.Calls.FromSqlRaw(query, parameters);
		}

		public async Task<IQueryable<Call>> GetAllAsync(int skip, int take)
		{
			var items = await _context.Calls.Skip(skip).Take(take).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.Calls.CountAsync();
		}

		public async Task<Call> GetAsync(int id)
		{
			var item = await _context.Calls.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Call entity)
		{
			await _context.Calls.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Calls.FirstOrDefault(item => item.Id == id);
			_context.Calls.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Call entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Calls.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
