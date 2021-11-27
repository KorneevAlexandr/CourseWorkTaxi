using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Data;
using Taxi.UI.Filters;
using Taxi.UI.Models.Brands;
using Taxi.UI.Models.CarModels;
using Taxi.UI.Models.Cars;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Controllers
{
	public class CarController : Controller
	{
		private const int AMOUNT = 8;
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
				CurrentPage = _currentPage + 1,
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

			var positions = await _poisitionService.GetAllAsync();
			var driverPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Водитель.ToString()));
			var mechanicPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Механик.ToString())); 

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

		public async Task<IActionResult> Create(CarCreateUpdateViewModel model)
		{
			var car = new CarDto
			{
				ModelId = model.ModelId,
				DriverId = model.DriverId,
				MechanicId = model.MechanicId,
				BodyNumber = model.BodyNumber,
				EngineNumber = model.EngineNumber,
				IssueYear = model.IssueYear,
				Mileage = model.Mileage,
				TariffId = model.TariffId,
				LastTI = model.LastTI,
				RegistrationNumber = model.RegistrationNumber,
			};
			var modelCar = await _modelService.GetAsync(model.ModelId);
			var brand = await _brandService.GetAsync(modelCar.BrandId);
			await _carService.CreateAsync(car);
			return Redirect($"~/Car/Index?brandId={brand.Id}");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			var car = await _carService.GetAsync(id.Value);
			var model = new CarViewModel
			{
				Id = car.Id,
				ModelName = car.ModelName,
				DriverFullName = car.DriverFullName,
				MechanicFullName = car.MechanicFullName,
				BodyNumber = car.BodyNumber,
				EngineNumber = car.EngineNumber,
				IssueYear = car.IssueYear,
				Mileage = car.Mileage,
				RegistrationNumber = car.RegistrationNumber,
				LastTI = car.LastTI,
				TariffName = car.TariffName,
				Price = car.Price,
			};
			return View(model);
		}

		[DeleteExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> DeleteCar(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var car = await _carService.GetAsync(id.Value);
			var model = await _modelService.GetAsync(car.ModelId);
			await _carService.DeleteAsync(id.Value);
			return Redirect($"~/Car/Index?brandId={model.BrandId}");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			var car = await _carService.GetAsync(id.Value);
			var modelCar = await _modelService.GetAsync(car.ModelId);
			var brand = await _brandService.GetAsync(modelCar.BrandId);
			var models = await _modelService.GetAllByBrandAsync(brand.Id);
			var tariffs = await _tariffService.GetAllAsync();

			var positions = await _poisitionService.GetAllAsync();
			var driverPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Водитель.ToString())); // тут!
			var mechanicPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Механик.ToString())); // и тут!

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
				BodyNumber = car.BodyNumber,
				EngineNumber = car.EngineNumber,
				RegistrationNumber = car.RegistrationNumber,
				DriverId = car.DriverId,
				IssueYear = car.IssueYear,
				LastTI = car.LastTI,
				Mileage = car.Mileage,
				MechanicId = car.MechanicId,
				ModelId = car.ModelId,
				Id = car.Id,
				TariffId = car.TariffId,
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

		[HttpPost]
		public async Task<IActionResult> Update(CarCreateUpdateViewModel model)
		{
			var car = new CarDto
			{
				Id = model.Id,
				ModelId = model.ModelId,
				DriverId = model.DriverId,
				MechanicId = model.MechanicId,
				BodyNumber = model.BodyNumber,
				EngineNumber = model.EngineNumber,
				IssueYear = model.IssueYear,
				Mileage = model.Mileage,
				TariffId = model.TariffId,
				LastTI = model.LastTI,
				RegistrationNumber = model.RegistrationNumber,
			};
			var modelCar = await _modelService.GetAsync(model.ModelId);
			var brand = await _brandService.GetAsync(modelCar.BrandId);
			await _carService.UpdateAsync(car);
			return Redirect($"~/Car/Index?brandId={brand.Id}");
		}

		public IActionResult ShowTICars()
		{
			var cars = _carService.GetAllForTI();
			var model = cars.Select(x => new CarViewModel
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
			}).ToList();

			return View(model);
		}

		public async Task<IActionResult> UpdateTI(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var car = await _carService.GetAsync(id.Value);

			car.LastTI = DateTime.Now;
			await _carService.UpdateAsync(car);
			return RedirectToAction("ShowTICars");
		}
	}
}
