using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.UI.Data;
using Taxi.UI.Models.Calls;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Controllers
{
	public class CallController : Controller
	{
		private const int AMOUNT = 20;
		private int _currentPage;

		private readonly ICallService _callService;
		private readonly IPositionService _positionService;
		private readonly IEmployeeService _employeeService;
		private readonly ITariffService _tariffService;
		private readonly UserManager<User> _userManager;

		public CallController(IPositionService positionService, IEmployeeService employeeService,
			ICallService callService, ITariffService tariffService, UserManager<User> userManager)
		{
			_callService = callService;
			_employeeService = employeeService;
			_tariffService = tariffService;
			_positionService = positionService;
			_userManager = userManager;
			_currentPage = 0;
		}

		public async Task<IActionResult> Index(int? page, int? tariffId, DateTime? date, int? driverId, int? dispatherId)
		{
			// для даты обработки нету, она принимается сервисом как Nullable
			_currentPage = page == null ? 0 : page.Value - 1;
			var selectedTariffId = tariffId == null ? 0 : tariffId.Value;
			var selectedDriverId = driverId == null ? 0 : driverId.Value;
			var selectedDispatherId = dispatherId == null ? 0 : dispatherId.Value;

			var countCalls = await _callService.GetCountAsync(selectedTariffId, date, selectedDriverId, selectedDispatherId);
			var calls = await _callService.GetAllAsync(selectedTariffId, date, selectedDriverId, selectedDispatherId,
				_currentPage * AMOUNT, AMOUNT);
			
			var positions = await _positionService.GetAllAsync();
			var driverPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Водитель.ToString()));
			var dispatherPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Диспетчер.ToString()));
			var countDrivers = await _employeeService.GetCountAsync(driverPosition.Id, 0);
			var countDispathers = await _employeeService.GetCountAsync(dispatherPosition.Id, 0);

			var drivers = await _employeeService.GetAllAsync(0, driverPosition.Id, 0, countDrivers);
			var dispathers = await _employeeService.GetAllAsync(0, dispatherPosition.Id, 0, countDispathers);

			var tariffs = await _tariffService.GetAllAsync();

			var modelTariffs = new List<TariffViewModel>
				{ new TariffViewModel { Id = 0, Name = "Любой" } };
			var modelDrivers = new List<EmployeesSelectViewModel>
				{ new EmployeesSelectViewModel { Id = 0, FullName = "Любой" } };
			var modelDispathers = new List<EmployeesSelectViewModel>
				{ new EmployeesSelectViewModel { Id = 0, FullName = "Любой" } };
			
			modelDrivers.AddRange(drivers.Select(x => new EmployeesSelectViewModel
			{
				Id = x.Id,
				FullName = $"{x.Surname} {x.Name}",
			}));
			modelDispathers.AddRange(dispathers.Select(x => new EmployeesSelectViewModel
			{
				Id = x.Id,
				FullName = $"{x.Surname} {x.Name}",
			}));
			modelTariffs.AddRange(tariffs.Select(x => new TariffViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}));

			var model = new CallCollectionViewModel
			{
				CountPages = countCalls % AMOUNT == 0
					? (int)(countCalls / AMOUNT) : (int)(countCalls / AMOUNT) + 1,
				CurrentPage = _currentPage + 1,
				Drivers = modelDrivers,
				Dispathers = modelDispathers,
				Tariffs = modelTariffs,

				TariffId = selectedTariffId,
				Date = date,
				DispatherId = selectedDispatherId,
				DriverId = selectedDriverId,

				Calls = calls.Select(x => new CallViewModel
				{
					Id = x.Id,
					Phone = x.Phone,
					Price = x.Price,
					CallDateTime = x.CallDateTime,
					StartStreet = x.StartStreet,
					EndStreet = x.EndStreet,
					StartHomeNumber = x.StartHomeNumber,
					EndHomeNumber = x.EndHomeNumber,
					DriverFullName = x.DriverFullName,
					DispatherFullName = x.DispatherFullName,
				}).ToList(),
			};

			return View(model);
		}

		public async Task<IActionResult> PopularAddresses()
		{
			var startAddresses = await _callService.GetPopularStartStreets();
			var endAddresses = await _callService.GetPopularEndStreets();

			var model = new CallAddressCollectionViewModel
			{
				StartAddresses = startAddresses.Select(x => new CallAddressViewModel
				{
					Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.MonthNumber),
					Address = x.AddressName,
					CountAddresses = x.CountCalls,
				}).ToList(),

				EndAddresses = endAddresses.Select(x => new CallAddressViewModel
				{
					Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.MonthNumber),
					Address = x.AddressName,
					CountAddresses = x.CountCalls,
				}).ToList(),
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var call = await _callService.GetAsync(id);
			var model = new CallViewModel
			{
				Id = call.Id,
				DispatherFullName = call.DispatherFullName,
				DriverFullName = call.DriverFullName,
				EndStreet = call.EndStreet,
				CallDateTime = call.CallDateTime,
				EndHomeNumber = call.EndHomeNumber,
				Phone = call.Phone,
				Price = call.Price,
				StartHomeNumber = call.StartHomeNumber,
				StartStreet = call.StartStreet,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteCall(int id)
		{
			await _callService.DeleteAsync(id);
			return Redirect("~/Call/Index");
		}

		public async Task<IActionResult> DispatcherCalls(int? page, int? tariffId, DateTime? date, int? driverId)
		{
			
			_currentPage = page == null ? 0 : page.Value - 1;
			var selectedTariffId = tariffId == null ? 0 : tariffId.Value;
			var selectedDriverId = driverId == null ? 0 : driverId.Value;
			date = date == null ? DateTime.Now : date;

			var user = await _userManager.Users.FirstOrDefaultAsync(us => us.Email.Equals(User.Identity.Name));
			var selectedDispatherId = user.EmployeeId;

			var countCalls = await _callService.GetCountAsync(selectedTariffId, date, selectedDriverId, selectedDispatherId);
			var calls = await _callService.GetAllAsync(selectedTariffId, date, selectedDriverId, selectedDispatherId,
				_currentPage * AMOUNT, AMOUNT);

			var positions = await _positionService.GetAllAsync();
			var driverPosition = positions.FirstOrDefault(x => x.Name.Equals(DefaultPositions.Водитель.ToString()));
			var countDrivers = await _employeeService.GetCountAsync(driverPosition.Id, 0);

			var drivers = await _employeeService.GetAllAsync(0, driverPosition.Id, 0, countDrivers);

			var tariffs = await _tariffService.GetAllAsync();

			var modelTariffs = new List<TariffViewModel>
				{ new TariffViewModel { Id = 0, Name = "Любой" } };
			var modelDrivers = new List<EmployeesSelectViewModel>
				{ new EmployeesSelectViewModel { Id = 0, FullName = "Любой" } };

			modelDrivers.AddRange(drivers.Select(x => new EmployeesSelectViewModel
			{
				Id = x.Id,
				FullName = $"{x.Surname} {x.Name}",
			}));
			modelTariffs.AddRange(tariffs.Select(x => new TariffViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}));

			var model = new CallCollectionViewModel
			{
				CountPages = countCalls % AMOUNT == 0
					? (int)(countCalls / AMOUNT) : (int)(countCalls / AMOUNT) + 1,
				CurrentPage = _currentPage + 1,
				Drivers = modelDrivers,
				Tariffs = modelTariffs,

				TariffId = selectedTariffId,
				Date = date,
				DispatherId = selectedDispatherId,
				DriverId = selectedDriverId,

				Calls = calls.Select(x => new CallViewModel
				{
					Id = x.Id,
					Phone = x.Phone,
					Price = x.Price,
					CallDateTime = x.CallDateTime,
					StartStreet = x.StartStreet,
					EndStreet = x.EndStreet,
					StartHomeNumber = x.StartHomeNumber,
					EndHomeNumber = x.EndHomeNumber,
					DriverFullName = x.DriverFullName,
					DispatherFullName = x.DispatherFullName,
				}).ToList(),
			};

			return View(model);
		}
	}
}
