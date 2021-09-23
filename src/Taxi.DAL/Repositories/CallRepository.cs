using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

		public async Task<IQueryable<Call>> GetAllAsync(int tariffId, DateTime? day, int driverId, int dispatherId, int skip, int take)
		{
			var calls = Filter(tariffId, day, driverId, dispatherId);
			var items = await calls.Skip(skip).Take(take).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<int> GetCountAsync(int tariffId, DateTime? day, int driverId, int dispatherId)
		{
			var items = Filter(tariffId, day, driverId, dispatherId);
			return await items.CountAsync();
		}

		private IQueryable<Call> Filter(int tariffId, DateTime? day, int driverId, int dispatherId)
		{
			IQueryable<Call> calls = _context.Calls.Include(x => x.Car);

			if (tariffId != 0)
			{
				calls = calls.Where(x => x.Car.TariffId == tariffId);
			}
			if (day != null)
			{
				var endDay = day.Value.AddHours(23).AddMinutes(59);
				calls = calls.Where(x => x.CallDateTime >= day && x.CallDateTime <= endDay);
			}
			if (driverId != 0)
			{
				calls = calls.Where(x => x.Car.DriverId == driverId);
			}
			if (dispatherId != 0)
			{
				calls = calls.Where(x => x.DispatherId == dispatherId);
			}

			return calls;
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
