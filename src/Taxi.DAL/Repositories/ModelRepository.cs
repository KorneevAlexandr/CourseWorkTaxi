using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class ModelRepository : IModelRepository
	{
		private readonly TaxiContext _context;

		public ModelRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<IQueryable<Model>> GetAllByBrand(int brandId)
		{
			var items = await _context.Models.Where(x => x.BrandId == brandId).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<Model> GetAsync(int id)
		{
			var item = await _context.Models.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Model entity)
		{
			await _context.Models.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Models.FirstOrDefault(item => item.Id == id);
			_context.Models.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Model entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Models.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
