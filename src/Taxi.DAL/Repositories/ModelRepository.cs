using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	internal class ModelRepository : IModelRepository
	{
		private readonly TaxiContext _context;

		public ModelRepository(TaxiContext context)
		{
			_context = context;
		}

		public async Task<IQueryable<Model>> GetAllByBrandAsync(int brandId)
		{
			var items = await _context.Models.Include(x => x.Brand).Where(x => x.BrandId == brandId).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<Model> GetAsync(int id)
		{
			var item = await _context.Models.Include(x => x.Brand).FirstOrDefaultAsync(x => x.Id == id);
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
			item.Name = entity.Name;
			item.HP = entity.HP;
			item.Fuel = entity.Fuel;
			item.Body = entity.Body;
			item.Price = entity.Price;
			item.BrandId = entity.BrandId;

			_context.Models.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
