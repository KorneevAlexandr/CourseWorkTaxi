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

			if (ReadAndDeleteCookies(ref model))
			{
				ModelState.AddModelError("", "К сожалению, по выбранному вами тарифу машин нет.. Выберите другой тариф");
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> IndexCall(UserCallViewModel model)
		{
			CarDto car;
			try
			{
				car = await _carService.GetRandomCarByTariff(model.TariffId);
			}
			catch
			{
				WriteCookies(model.Phone, model.StartStreet, model.EndStreet, model.StartHomeNumber.ToString(), model.EndHomeNumber.ToString());
				return RedirectToAction("Index");
			}

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

			DeleteCookies();
			await _callService.CreateAsync(call);
			model.IsSuccessed = true;
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		private void WriteCookies(string phone, string startStreet, string endStreet, string startHome, string endHome)
		{
			HttpContext.Response.Cookies.Append("StartStreet", startStreet);
			HttpContext.Response.Cookies.Append("EndStreet", endStreet);
			HttpContext.Response.Cookies.Append("StartHome", startHome.ToString());
			HttpContext.Response.Cookies.Append("EndHome", endHome.ToString());
			HttpContext.Response.Cookies.Append("Phone", phone);
		}

		private bool ReadAndDeleteCookies(ref UserCallViewModel model)
		{
			if (!HttpContext.Request.Cookies.Keys.Contains("Phone"))
			{
				return false;
			}

			HttpContext.Request.Cookies.TryGetValue("Phone", out string phone);
			HttpContext.Request.Cookies.TryGetValue("StartStreet", out string startStreet);
			HttpContext.Request.Cookies.TryGetValue("StartHome", out string startHome);
			HttpContext.Request.Cookies.TryGetValue("EndStreet", out string endStreet);
			HttpContext.Request.Cookies.TryGetValue("EndHome", out string endHome);

			model.Phone = phone;
			model.StartStreet = startStreet;
			model.EndStreet = endStreet;
			model.StartHomeNumber = Convert.ToInt32(startHome);
			model.EndHomeNumber = Convert.ToInt32(endHome);

			DeleteCookies();

			return true;
		}

		private void DeleteCookies()
		{
			HttpContext.Response.Cookies.Delete("Phone");
			HttpContext.Response.Cookies.Delete("StartStreet");
			HttpContext.Response.Cookies.Delete("StartHome");
			HttpContext.Response.Cookies.Delete("EndStreet");
			HttpContext.Response.Cookies.Delete("EndHome");
		}
	}
}
