using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.UI.Models.Accounts;

namespace Taxi.UI.Controllers
{
	public class AccountController : Controller
	{
		private readonly IEmployeeService _employeeService;

		public AccountController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		public IActionResult Index()
		{
			return RedirectToAction("Login");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(AccountViewModel account)
		{
			if (ModelState.IsValid)
			{
				var employeeId = await _employeeService.EmployeeIsAutorizeAsync(account.Login, account.Password);
				if (employeeId != 0)
				{
					var employee = await _employeeService.GetAsync(employeeId);
					await Authenticate(account.Login, employee.PositionName);
					return RedirectToAction("Index", "Home");
					//return FindUrlAction(user);
				}
			}

			return View(account);
		}

		private async Task Authenticate(string login, string positionName)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
			};

			var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
		}

	}
}
