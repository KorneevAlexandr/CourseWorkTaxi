using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.BLL.Services
{
	public class CarService : ICarService
	{
		private readonly ICarRepository _carRepository;

		public CarService(string connectionString)
		{
			_carRepository = new CarRepository(connectionString);
		}

		public async Task<IEnumerable<CarDto>> GetAllAsync(int brandId, int mileage, int price, int issueYear, int skip, int take)
		{
			if (brandId == 0 && mileage == 0 && price == 0 && issueYear == 0)
			{
				var simpleItems = await _carRepository.GetAllAsync(skip, take);
				return simpleItems.Select(x => ItemConvert(x));
			}
			var items = await _carRepository.GetAllAsync(brandId, mileage, price, issueYear, skip, take);
			return items.Select(x => ItemConvert(x));
		}

		public async Task<int> GetCountAsync(int brandId, int mileage, int price, int issueYear)
		{
			if (brandId == 0 && mileage == 0 && price == 0 && issueYear == 0)
			{
				return await _carRepository.GetCountAsync();
			}
			return await _carRepository.GetCountAsync(brandId, mileage, price, issueYear);
		}

		public IEnumerable<CarDto> GetAllForTI()
		{
			var now = DateTime.Now; 
			var start = new DateTime(now.Year - 1, now.Month - 1, now.Day); // 2020 08 23
			var end = new DateTime(start.Year, start.Month + 1, start.Day); // 2020 09 23

			var cars = _carRepository.GetAllForTI(start, end);
			return cars.Select(x => ItemConvert(x));
		}

		private CarDto ItemConvert(Car car)
		{
			return new CarDto
			{
				Id = car.Id,
				BodyNumber = car.BodyNumber,
				DriverId = car.DriverId,
				EngineNumber = car.EngineNumber,
				IssueYear = car.IssueYear,
				LastTI = car.LastTI,
				MechanicId = car.MechanicId,
				Mileage = car.Mileage,
				ModelId = car.ModelId,
				RegistrationNumber = car.RegistrationNumber,
				TariffId = car.TariffId,
			};
		}

		private Car ItemConvert(CarDto car)
		{
			return new Car
			{
				Id = car.Id,
				BodyNumber = car.BodyNumber,
				DriverId = car.DriverId,
				EngineNumber = car.EngineNumber,
				IssueYear = car.IssueYear,
				LastTI = car.LastTI,
				MechanicId = car.MechanicId,
				Mileage = car.Mileage,
				ModelId = car.ModelId,
				RegistrationNumber = car.RegistrationNumber,
				TariffId = car.TariffId,
			};
		}
	}
}
