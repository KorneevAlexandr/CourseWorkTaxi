using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Data;
using Taxi.DAL.Domain;
using Taxi.DataInitialization.Interfaces;
using Taxi.DataInitialization.Options;

namespace Taxi.DataInitialization.Initializers
{
	internal class CarInitializer : IInitializer
	{
		private readonly int _minYear = 2010;
		private readonly char[] _alphabet = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
														 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L',
														 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

		private readonly TaxiContext _context;
		private readonly int _count;
		private readonly Random _random;

		private List<int> _tariffIds;
		private List<int> _modelIds;
		private List<int> _driverIds;
		private List<int> _mechanicIds;

		public CarInitializer(TaxiContext context)
		{
			_context = context;
			_count = DataInitializeOptions.CountCars;
			_random = new Random();
		}

		public bool Initialize { get => DataInitializeOptions.Initialize; }

		public async Task InitializeAsync()
		{
			if (!Initialize || _count <= 0)
			{
				return;
			}

			await GetIds();

			var cars = new List<Car>();
			for (int i = 0; i < _count; i++)
			{
				cars.Add(GenerateCar());
			}
			await _context.Cars.AddRangeAsync(cars);
			await _context.SaveChangesAsync();

			await Task.CompletedTask;
		}

		private async Task GetIds()
		{
			_tariffIds = await _context.Tariffs.Select(tariff => tariff.Id).ToListAsync();
			_modelIds = await _context.Models.Select(model => model.Id).ToListAsync();

			var mechanicPosition = await _context.Positions.FirstOrDefaultAsync(position => position.Name.Equals(Positions.Механик.ToString()));
			var driverPosition = await _context.Positions.FirstOrDefaultAsync(position => position.Name.Equals(Positions.Водитель.ToString()));

			_mechanicIds = await _context.Employees.Where(employee => employee.PositionId == mechanicPosition.Id)
				.Select(mechanic => mechanic.Id).ToListAsync();
			_driverIds = await _context.Employees.Where(employee => employee.PositionId == driverPosition.Id)
				.Select(driver => driver.Id).ToListAsync();
		}

		private Car GenerateCar()
		{
			var registrationNumber = GenerateRegistrationNumber();
			while (_context.Cars.Select(car => car.RegistrationNumber).Contains(registrationNumber))
			{
				registrationNumber = GenerateRegistrationNumber();
			}
			var engineNumber = _random.Next(100000, 10000000);
			var bodyNumber = _random.Next(100000, 10000000);
			var issueYear = _random.Next(_minYear, DateTime.Now.Year);
			var mileage = (DateTime.Now.Year - issueYear) * _random.Next(20000, 50000);
			var dateTimeTI = new DateTime(_random.Next(DateTime.Now.Year - 2, DateTime.Now.Year), _random.Next(1, 13), _random.Next(1, 29));

			var driverId = _driverIds[_random.Next(0, _driverIds.Count)];
			var mechanicId = _mechanicIds[_random.Next(0, _mechanicIds.Count)];
			var tariffId = _tariffIds[_random.Next(0, _tariffIds.Count)];
			var modelId = _modelIds[_random.Next(0, _modelIds.Count)];

			return new Car
			{
				RegistrationNumber = registrationNumber,
				EngineNumber = engineNumber,
				BodyNumber = bodyNumber,
				IssueYear = issueYear,
				Mileage = mileage,
				LastTI = dateTimeTI,
				DriverId = driverId,
				MechanicId = mechanicId,
				ModelId = modelId,
				TariffId = tariffId,
			};
		}

		private string GenerateRegistrationNumber()
		{
			var startPart = "";
			for (int i = 0; i < 4; i++)
			{
				startPart += _random.Next(0, 10).ToString();
			}

			var letterPart = string.Concat(_alphabet[_random.Next(0, _alphabet.Length)],
											_alphabet[_random.Next(0, _alphabet.Length)]);

			var lastPart = _random.Next(0, 10);

			return string.Concat(startPart, " ", letterPart, "-", lastPart);
		}

		private enum Positions : byte
		{
			Механик,
			Водитель
		}
	}
}
