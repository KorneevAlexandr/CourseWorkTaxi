﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.UI.Data;
using Taxi.UI.Models.Users;

namespace Taxi.UI.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IEmployeeService _employeeService;
		private readonly SignInManager<User> _signInManager;

		public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IEmployeeService employeeService)
		{
			_employeeService = employeeService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = _userManager.Users.FirstOrDefault(us => us.Email.Equals(User.Identity.Name));
			var employee = await _employeeService.GetAsync(user.EmployeeId);
			var role = User.Claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).FirstOrDefault();

			var roleName = role == null ? DefaultRoles.Employee.ToString() : role.Value;

			var model = new UserViewModel
			{
				RoleName = roleName,
				Email = user.Email,
				Name = employee.Name,
				Surname = employee.Surname,
				StartWork = employee.DateStartOfWork,
				PositionName = employee.PositionName,
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (model.NewPassword == null || model.OldPassword == null)
			{
				ModelState.AddModelError("", "Заполните все поля");
				return View(model);
			}
			var user = _userManager.Users.FirstOrDefault(us => us.Email.Equals(User.Identity.Name));

			var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("", "Старый пароль неверный!");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ChangeEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
		{
			if (model.NewEmail == null || model.Password == null)
			{
				ModelState.AddModelError("", "Заполните все поля");
				return View(model);
			}

			var user = _userManager.Users.FirstOrDefault(us => us.Email.Equals(User.Identity.Name));
			user.Email = model.NewEmail;
			user.UserName = model.NewEmail;

			var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
			if (checkPassword)
			{
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					await _signInManager.PasswordSignInAsync(model.NewEmail, model.Password, true, false);
					return RedirectToAction("Index");
				}
				else
				{
					ModelState.AddModelError("", "Указанный email уже занят!");
				}
			}
			else
			{
				ModelState.AddModelError("", "Неверный пароль");
			}

			return View(model);
		}

	}
}
