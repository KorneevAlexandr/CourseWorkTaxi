using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	public class PositionRepository : ICompleteRepository<Position>
	{
		private readonly TaxiContext _context;

		public PositionRepository(string connectionString)
		{
			_context = new TaxiContext(connectionString);
		}

		public async Task<IQueryable<Position>> GetAllAsync()
		{
			var items = await _context.Positions.ToListAsync();
			return items.AsQueryable();
		}
		
		public async Task<Position> GetAsync(int id)
		{
			var item = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Position entity)
		{
			await _context.Positions.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Positions.FirstOrDefault(item => item.Id == id);
			_context.Positions.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Position entity)
		{
			var item = await GetAsync(entity.Id);
			item.Name = entity.Name;
			item.Description = entity.Description;
			_context.Positions.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
