using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Data;
using Taxi.UI.Models.Employees;
using Taxi.UI.Models.Positions;

namespace Taxi.UI.Controllers
{
	public class EmployeeController : Controller
	{
		private const int AMOUNT = 2;
		private int _currentPage;

		private readonly IEmployeeService _employeeService;
		private readonly IPositionService _positionService;
		private readonly UserManager<User> _userManager;

		public EmployeeController(IEmployeeService employeeService, IPositionService positionService, UserManager<User> userManager)
		{
			_employeeService = employeeService;
			_positionService = positionService;
			_userManager = userManager;
			_currentPage = 0;
		}

		public async Task<IActionResult> Index(int? page, int? positionId, int? yearStanding)
		{
			_currentPage = page == null ? 0 : page.Value - 1;
			var selectedPositionId = positionId == null ? 0 : positionId.Value;
			var selectedYearStanding = yearStanding == null ? 0 : yearStanding.Value;

			var countEmployees = await _employeeService.GetCountAsync(selectedPositionId, selectedYearStanding);
			var employees = await _employeeService.GetAllAsync(
				selectedYearStanding, selectedPositionId, _currentPage * AMOUNT, AMOUNT);
			var positions = await _positionService.GetAllAsync();
			
			var modelPositions = new List<PositionViewModel>
				{ new PositionViewModel { Id = 0, Name = "Любой" } };
			modelPositions.AddRange(positions.Select(x => new PositionViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}));

			var model = new EmployeeCollectionViewModel
			{
				YearStanding = selectedYearStanding,
				SelectedPositionId = selectedPositionId,
				CountPages = countEmployees % AMOUNT == 0
					? (int)(countEmployees / AMOUNT) : (int)(countEmployees / AMOUNT) + 1,
				Employees = employees.Select(x => new EmployeeViewModel
				{
					Id = x.Id,
					Name = x.Name,
					Surname = x.Surname,
					PositionName = x.PositionName,
					DateStartOfWork = x.DateStartOfWork,
					Registered = _userManager.Users.Where(user => user.EmployeeId == x.Id).Any(),
				}).ToList(),
				Positions = modelPositions,
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var positions = await _positionService.GetAllAsync();
			var model = new EmployeeViewModel
			{
				Positions = positions.Select(x => new PositionViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList(),
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel model)
		{
			var employee = new EmployeeDto
			{
				Id = model.Id,
				Name = model.Name,
				Surname = model.Surname,
				DateStartOfWork = model.DateStartOfWork,
				PositionId = model.PositionId,
			};
			await _employeeService.CreateAsync(employee);
			return Redirect($"~/Employee/Index?positionId={model.PositionId}");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var employee = await _employeeService.GetAsync(id.Value);
			await _employeeService.DeleteAsync(id.Value);
			return Redirect($"~/Employee/Index?positionId={employee.PositionId}");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var positions = await _positionService.GetAllAsync();
			var employee = await _employeeService.GetAsync(id.Value);
			var model = new EmployeeViewModel
			{
				Id = employee.Id,
				Name = employee.Name,
				PositionId = employee.PositionId,
				Surname = employee.Surname,
				DateStartOfWork = employee.DateStartOfWork,
				Positions = positions.Select(x => new PositionViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList(),
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(EmployeeViewModel model)
		{
			var employee = new EmployeeDto
			{
				Id = model.Id,
				Name = model.Name,
				Surname = model.Surname,
				DateStartOfWork = model.DateStartOfWork,
				PositionId = model.PositionId,
			};
			await _employeeService.UpdateAsync(employee);
			return Redirect($"~/Employee/Index?positionId={employee.PositionId}");
		}
	}
}
