using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	internal class EmployeeRepository : IEmployeeRepository
	{
		private readonly TaxiContext _context;

		public EmployeeRepository(TaxiContext context)
		{
			_context = context;
		}

		public async Task<IQueryable<Employee>> GetAllAsync(
			Func<IQueryable<Employee>, IQueryable<Employee>> func)
		{
			var items = await (func(_context.Employees.AsQueryable())).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<IQueryable<Employee>> GetAllAsync(int skip, int take)
		{
			var items = await _context.Employees.Skip(skip).Take(take).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<Employee> GetLastAsync()
		{
			return await _context.Employees.LastAsync();
		}

		public async Task<int> GetCountAsync(Func<IQueryable<Employee>, IQueryable<Employee>> predicate)
		{
			return await (predicate(_context.Employees)).CountAsync();
			// return await _context.Employees.CountAsync();
		}

		public async Task<Employee> GetAsync(int id)
		{
			var item = await _context.Employees.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task CreateAsync(Employee entity)
		{
			await _context.Employees.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var deletedItem = _context.Employees.FirstOrDefault(item => item.Id == id);
			_context.Employees.Remove(deletedItem);
			await SaveChangesAsync();
		}

		public async Task UpdateAsync(Employee entity)
		{
			var item = await GetAsync(entity.Id);
			item.Name = entity.Name;
			item.Surname = entity.Surname;
			item.PositionId = entity.PositionId;
			item.DateStartOfWork = entity.DateStartOfWork;
			_context.Employees.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
