using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class CarRepository : ICarRepository
	{
		private readonly TaxiContext _context;

		public CarRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public IQueryable<Car> GetAllByQuery(string query, params object[] parameters)
		{
			return _context.Cars.FromSqlRaw(query, parameters);
		}

		public async Task<IQueryable<Car>> GetAllAsync(int skip, int take)
		{
			var items = await _context.Cars.Skip(skip).Take(take).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.Cars.CountAsync();
		}

		public async Task<Car> GetAsync(int id)
		{
			var item = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Car entity)
		{
			await _context.Cars.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Cars.FirstOrDefault(item => item.Id == id);
			_context.Cars.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Car entity)
		{
			var item = await GetAsync(entity.Id);
			item = entity;
			_context.Cars.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
