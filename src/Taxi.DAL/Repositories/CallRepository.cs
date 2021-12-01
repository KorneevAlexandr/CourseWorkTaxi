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

		public CallRepository(TaxiContext context)
		{
			_context = context;
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

		public async Task<Dictionary<string, int>> GetPopularStartStreet(int year, int month)
		{
			var startStreets = await _context.Calls
				.Where(x => x.CallDateTime.Year == year && x.CallDateTime.Month == month)
				.Select(x => x.StartStreet).ToListAsync();

			if (startStreets == null || startStreets.Count == 0)
			{
				return new Dictionary<string, int>();
			}

			var startStreetsCounts = startStreets.Select(x => new
				{ 
					Name = x,
					Count = startStreets.Count(y => y.Equals(x)),
				}).OrderByDescending(x => x.Count);
			var startStreet = startStreetsCounts.FirstOrDefault().Name;
			var startStreetCount = startStreetsCounts.FirstOrDefault().Count;

			return new Dictionary<string, int>
			{
				{ startStreet, startStreetCount },
			};
		}

		public async Task<Dictionary<string, int>> GetPopularEndStreet(int year, int month)
		{
			var endStreets = await _context.Calls
				.Where(x => x.CallDateTime.Year == year && x.CallDateTime.Month == month)
				.Select(x => x.EndStreet).ToListAsync();

			if (endStreets == null || endStreets.Count == 0)
			{
				return new Dictionary<string, int>();
			}

			var endStreetsCounts = endStreets.Select(x => new
			{
				Name = x,
				Count = endStreets.Count(y => y.Equals(x)),
			}).OrderByDescending(x => x.Count);
			var endStreet = endStreetsCounts.FirstOrDefault().Name;
			var endStreetCount = endStreetsCounts.FirstOrDefault().Count;

			return new Dictionary<string, int>
			{
				{ endStreet, endStreetCount },
			};
		}

		private IQueryable<Call> Filter(int tariffId, DateTime? day, int driverId, int dispatherId)
		{
			IQueryable<Call> calls = _context.Calls.Include(x => x.Car).Include(x => x.Dispather);

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
			var item = await _context.Calls.Include(x => x.Car).Include(x => x.Dispather).FirstOrDefaultAsync(x => x.Id == id);
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
			_context.Calls.Update(entity);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
