using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.UI.Models.Brands;
using Taxi.UI.Models.CarModels;
using Taxi.UI.Models.Cars;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Controllers
{
	public class CarController : Controller
	{
		private const int AMOUNT = 2;
		private int _currentPage;

		private readonly IPositionService _poisitionService;
		private readonly ICarService _carService;
		private readonly IModelService _modelService;
		private readonly IBrandService _brandService;
		private readonly IEmployeeService _employeeService;
		private readonly ITariffService _tariffService;

		public CarController(ICarService carService, IModelService modelService, ITariffService tariffService, 
			IBrandService brandService, IPositionService positionService, IEmployeeService employeeService)
		{
			_carService = carService;
			_modelService = modelService;
			_brandService = brandService;
			_poisitionService = positionService;
			_employeeService = employeeService;
			_tariffService = tariffService;
		}

		public async Task<IActionResult> Index(int? page, int? brandId, int? mileage, int? price, int? issueYear)
		{
			_currentPage = page == null ? 0 : page.Value - 1;
			int selectedBrandId = brandId == null ? 0 : brandId.Value;
			int selectedMileage = mileage == null ? 0 : mileage.Value;
			int selectedPrice = price == null ? 0 : price.Value;
			int selectedIssueYear = issueYear == null ? 0 : issueYear.Value;

			var countCars = await _carService.GetCountAsync(
				selectedBrandId, selectedMileage, selectedPrice, selectedIssueYear);
			var cars = await _carService.GetAllAsync(
				selectedBrandId, selectedMileage, selectedPrice, selectedIssueYear,
				_currentPage * AMOUNT, AMOUNT);
			var brands = await _brandService.GetAllAsync();

			var modelBrands = new List<BrandViewModel>
				{ new BrandViewModel { Id = 0, Name = "Любой" } };
			modelBrands.AddRange(brands.Select(x => new BrandViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}));

			var model = new CarCollectionViewModel
			{
				BrandId = selectedBrandId,
				IssueYear = selectedIssueYear,
				Mileage = selectedMileage,
				Price = selectedPrice,
				CountPages = countCars % AMOUNT == 0
					? (int)(countCars / AMOUNT) : (int)(countCars / AMOUNT) + 1,
				Brands = modelBrands,

				Cars = cars.Select(x => new CarViewModel
				{
					Id = x.Id,
					ModelName = x.ModelName,
					DriverFullName = x.DriverFullName,
					MechanicFullName = x.MechanicFullName,
					BodyNumber = x.BodyNumber,
					EngineNumber = x.EngineNumber,
					IssueYear = x.IssueYear,
					Mileage = x.Mileage,
					RegistrationNumber = x.RegistrationNumber,
					LastTI = x.LastTI,
					TariffName = x.TariffName,
					Price = x.Price,
				}).ToList(),
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int? brandId)
		{
			if (brandId == null)
			{
				return RedirectToAction("Index");
			}

			var brand = await _brandService.GetAsync(brandId.Value);
			var models = await _modelService.GetAllByBrandAsync(brandId.Value);
			var tariffs = await _tariffService.GetAllAsync();

			// TODO : хардкод
			// добавить имена ролей на русском в конфигурационный файл
			var positions = await _poisitionService.GetAllAsync();
			var driverPosition = positions.FirstOrDefault(x => x.Name.Equals("Водитель")); // тут!
			var mechanicPosition = positions.FirstOrDefault(x => x.Name.Equals("Механик")); // и тут!

			if (driverPosition == null || mechanicPosition == null)
			{
				return NotFound();
			}

			var countDrivers = await _employeeService.GetCountAsync(driverPosition.Id, 0);
			var countMechanics = await _employeeService.GetCountAsync(mechanicPosition.Id, 0);
			var drivers = await _employeeService.GetAllAsync(0, driverPosition.Id, 0, countDrivers);
			var mechanics = await _employeeService.GetAllAsync(0, mechanicPosition.Id, 0, countMechanics);

			var model = new CarCreateUpdateViewModel
			{
				BrandName = brand.Name,
				Models = models.Select(x => new ModelSelectViewModel
				{ 
					Id = x.Id,
					Name = x.Name,
				}).ToList(),

				Tariffs = tariffs.Select(x => new TariffViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList(),
				
				Drivers = drivers.Select(x => new EmployeesSelectViewModel
				{
					Id = x.Id,
					FullName = $"{x.Surname} {x.Name}",
				}).ToList(),

				Mechanics = mechanics.Select(x => new EmployeesSelectViewModel
				{
					Id = x.Id,
					FullName = $"{x.Surname} {x.Name}",
				}).ToList(),
			};

			return View(model);
		}


	}
}
