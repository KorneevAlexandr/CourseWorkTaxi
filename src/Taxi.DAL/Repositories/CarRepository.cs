using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.DAL.Repositories
{
	internal class CarRepository : ICarRepository
	{
		private readonly TaxiContext _context;

		public CarRepository(TaxiContext context)
		{
			_context = context;
		}

		public async Task<IQueryable<Car>> GetAllAsync(int brandId, int mileage, int price, int issueYear, int skip, int take)
		{
			IQueryable<Car> cars = _context.Cars.Include(x => x.Model).Include(x => x.Mechanic).Include(x => x.Driver).Include(x => x.Tariff);

			if (brandId != 0)
			{
				cars = cars.Where(x => x.Model.BrandId == brandId);
			}
			if (mileage != 0)
			{
				cars = cars.Where(x => x.Mileage <= mileage);
			}
			if (price != 0)
			{
				cars = cars.Where(x => x.Model.Price >= price);
			}
			if (issueYear != 0)
			{
				cars = cars.Where(x => x.IssueYear >= issueYear);
			}

			var carsRes = await cars.Skip(skip).Take(take).ToListAsync();
			return carsRes.AsQueryable();
		}

		public async Task<int> GetCountAsync(int brandId, int mileage, int price, int issueYear)
		{
			IQueryable<Car> cars = _context.Cars.Include(x => x.Model);

			if (brandId != 0)
			{
				cars = cars.Where(x => x.Model.BrandId == brandId);
			}
			if (mileage != 0)
			{
				cars = cars.Where(x => x.Mileage <= mileage);
			}
			if (price != 0)
			{
				cars = cars.Where(x => x.Model.Price >= price);
			}
			if (issueYear != 0)
			{
				cars = cars.Where(x => x.IssueYear >= issueYear);
			}

			return await cars.CountAsync();
		}

		public IQueryable<Car> GetAllForTI(DateTime dateStart, DateTime dateEnd)
		{
			return _context.Cars.Include(x => x.Model).Include(x => x.Mechanic).Include(x => x.Driver).Include(x => x.Tariff)
				.Where(x => x.LastTI >= dateStart && x.LastTI <= dateEnd);
		}

		public async Task<IQueryable<Car>> GetAllAsync(int skip, int take)
		{
			var items = await _context.Cars.Include(x => x.Model).Include(x => x.Mechanic).Include(x => x.Driver).Include(x => x.Tariff)
				.Skip(skip).Take(take).ToListAsync();
			return items.AsQueryable();
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.Cars.CountAsync();
		}

		public async Task<Car> GetAsync(int id)
		{
			var item = await _context.Cars.Include(x => x.Model).Include(x => x.Mechanic).Include(x => x.Driver).Include(x => x.Tariff)
				.FirstOrDefaultAsync(x => x.Id == id);
			return item;
		}

		public async Task<Car> GetRandomCarByTariff(int tariffId)
		{
			var random = new Random();
			var carsByTariffId = _context.Cars.Where(x => x.TariffId == tariffId).Select(x => x.Id);
			var countSelectedCars = await carsByTariffId.CountAsync();

			if (countSelectedCars == 0)
			{
				throw new InvalidOperationException("Машины с заданном тарифом нет.");
			}

			var randomCarId = await carsByTariffId.Skip(random.Next(0, countSelectedCars))
				.Take(1).FirstOrDefaultAsync();

			return await GetAsync(randomCarId);
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
			item.BodyNumber = entity.BodyNumber;
			item.EngineNumber = entity.EngineNumber;
			item.DriverId = entity.DriverId;
			item.IssueYear = entity.IssueYear;
			item.LastTI = entity.LastTI;
			item.MechanicId = entity.MechanicId;
			item.ModelId = entity.ModelId;
			item.Mileage = entity.Mileage;
			item.TariffId = entity.TariffId;

			_context.Cars.Update(item);
			await SaveChangesAsync();
		}

		private async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
