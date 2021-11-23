using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace Taxi.UI.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IEmployeeService _employeeService;

		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
			IEmployeeService employeeService)
		{
			_employeeService = employeeService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public async Task<IActionResult> Register(int id)
		{
			var employee = await _employeeService.GetAsync(id);
			var model = new RegisterViewModel 
			{ 
				EmployeeId = id,
				DataEmployee = $"{employee.Surname} {employee.Name}, {employee.PositionName}",
			};

			return View(model);
		}

		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new User
				{
					Email = model.Email,
					UserName = model.Email,
					EmployeeId = model.EmployeeId,
				};

				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					var roleName = await GetUserRoleAsync(user.EmployeeId);

					await _userManager.AddToRoleAsync(user, roleName);
					return RedirectToAction("Index", "Employee");
				}
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return Redirect("~/Account/Login");
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (HttpContext.Request.QueryString.HasValue)
			{
				return Redirect("~/Account/Login");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(AccountViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "User");
				}
				else
				{
					ModelState.AddModelError("", "Неверный логин или пароль");
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		private async Task<string> GetUserRoleAsync(int employeeId)
		{
			var employee = await _employeeService.GetAsync(employeeId);
			if (employee.PositionName.Equals(DefaultPositions.Водитель.ToString()))
			{
				return DefaultRoles.Driver.ToString();
			}
			else if (employee.PositionName.Equals(DefaultPositions.Диспетчер.ToString()))
			{
				return DefaultRoles.Dispatcher.ToString();
			}
			else if (employee.PositionName.Equals(DefaultPositions.Администратор.ToString()))
			{
				return DefaultRoles.Admin.ToString();
			}
			else if (employee.PositionName.Equals(DefaultPositions.Механик.ToString()))
			{
				return DefaultRoles.Employee.ToString();
			}
			else if (employee.PositionName.Equals(DefaultPositions.Модератор.ToString()))
			{
				return DefaultRoles.CarManager.ToString();
			}

			return DefaultRoles.Employee.ToString();
		}

	}
}
