using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.UI.Data;
using Taxi.UI.Models.Accounts;
using Taxi.UI.Models.Users;

namespace Taxi.UI.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IEmployeeService _employeeService;

		public UserController(UserManager<User> userManager,IEmployeeService employeeService)
		{
			_employeeService = employeeService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = _userManager.Users.FirstOrDefault(us => us.Email.Equals(User.Identity.Name));
			var employee = await _employeeService.GetAsync(user.EmployeeId);

			var model = new UserViewModel
			{
				Email = user.Email,
				Name = employee.Name,
				Surname = employee.Surname,
				StartWork = employee.DateStartOfWork,
				PositionName = employee.PositionName,
			};

			return View(model);
		}

	}
}
