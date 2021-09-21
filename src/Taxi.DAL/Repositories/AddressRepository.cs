using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class AddressRepository : ICompleteRepository<Address>
	{
		private readonly TaxiContext _context;

		public AddressRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<IQueryable<Address>> GetAllAsync()
		{
			var items = await _context.Addresses.ToListAsync();
			return items.AsQueryable();
		}

		public async Task CreateAsync(Address entity)
		{
			await _context.Addresses.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Addresses.FirstOrDefault(item => item.Id == id);
			_context.Addresses.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task<Address> GetAsync(int id)
		{
			return await _context.Addresses.FirstAsync(item => item.Id == id);
		}

		public async Task UpdateAsync(Address entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Addresses.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
