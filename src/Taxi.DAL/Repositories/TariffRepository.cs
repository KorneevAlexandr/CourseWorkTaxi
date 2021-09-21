using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class TariffRepository : ICompleteRepository<Tariff>
	{
		private readonly TaxiContext _context;

		public TariffRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<IQueryable<Tariff>> GetAllAsync()
		{
			var items = await _context.Tariffs.ToListAsync();
			return items.AsQueryable();
		}

		public async Task<Tariff> GetAsync(int id)
		{
			var item = await _context.Tariffs.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Tariff entity)
		{
			await _context.Tariffs.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Tariffs.FirstOrDefault(item => item.Id == id);
			_context.Tariffs.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Tariff entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Tariffs.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
