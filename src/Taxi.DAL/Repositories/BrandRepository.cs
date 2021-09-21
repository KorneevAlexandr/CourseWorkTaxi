using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class BrandRepository : ICompleteRepository<Brand>
	{
		private readonly TaxiContext _context;

		public BrandRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<IQueryable<Brand>> GetAllAsync()
		{
			var items = await _context.Brands.ToListAsync();
			return items.AsQueryable();
		}

		public async Task CreateAsync(Brand entity)
		{
			await _context.Brands.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Brands.FirstOrDefault(item => item.Id == id);
			_context.Brands.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task<Brand> GetAsync(int id)
		{
			return await _context.Brands.FirstAsync(item => item.Id == id);
		}

		public async Task UpdateAsync(Brand entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Brands.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
