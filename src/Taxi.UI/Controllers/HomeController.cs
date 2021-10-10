using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Models;
using Taxi.UI.Models.Calls;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly IModelService _modelService;
		private readonly ICallService _callService;
		private readonly IPositionService _positionService;
		private readonly ICarService _carService;
		private readonly ITariffService _tariffService;
		private readonly IEmployeeService _employeeService;

		public HomeController(ICallService callService, ITariffService tariffService, IModelService modelService,
			ICarService carService, IEmployeeService employeeService, IPositionService positionService)
		{
			_modelService = modelService;
			_callService = callService;
			_tariffService = tariffService;
			_carService = carService;
			_employeeService = employeeService;
			_positionService = positionService;
		}

		public async Task<IActionResult> Index()
		{
			var tariffs = await _tariffService.GetAllAsync();
			var model = new UserCallViewModel
			{
				Tariffs = tariffs.Select(x => new TariffViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList(),
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> IndexCall(UserCallViewModel model)
		{
			var car = await _carService.GetRandomCarByTariff(model.TariffId);
			var modelCar = await _modelService.GetAsync(car.ModelId);
			var driver = await _employeeService.GetAsync(car.DriverId);
			var tariff = await _tariffService.GetAsync(model.TariffId);
			
			// TODO : хардкод
			// добавить имена ролей на русском в конфигурационный файл
			var positions = await _positionService.GetAllAsync();
			var dispatherPosition = positions.FirstOrDefault(x => x.Name.Equals("Диспетчер")); // и тут!

			var dispather = await _employeeService.GetRandomAsync(dispatherPosition.Id);

			var random = new Random();

			var viewModel = new SuccessUserCallViewModel
			{
				UserCall = model,
				DriverFullName = $"{driver.Surname} {driver.Name}",
				CarName = $"{modelCar.BrandName} {modelCar.Name}",
				RegistrationName = $"{car.RegistrationNumber}",
				TariffName = tariff.Name,
				Price = random.Next(2, 10) * tariff.Price,
				DispatherId = dispather.Id,
				DriverId = driver.Id,
				CarId = car.Id,
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> IndexCall(SuccessUserCallViewModel model)
		{
			var call = new CallDto
			{
				DispatherId = model.DispatherId,
				DriverId = model.DriverId,
				CarId = model.CarId,
				CallDateTime = DateTime.Now,
				Phone = model.UserCall.Phone,
				StartStreet = model.UserCall.StartStreet,
				EndStreet = model.UserCall.EndStreet,
				StartHomeNumber = model.UserCall.StartHomeNumber,
				EndHomeNumber = model.UserCall.EndHomeNumber,
				Price = model.Price,
			};

			await _callService.CreateAsync(call);
			model.IsSuccessed = true;
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
