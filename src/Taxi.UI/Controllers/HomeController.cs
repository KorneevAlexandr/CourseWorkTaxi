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

namespace Taxi.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly IService<BrandDto> _brandService;

		public HomeController(IService<BrandDto> brandService)
		{
			_brandService = brandService;
		}

		public async Task<IActionResult> Index()
		{
			var items = await _brandService.GetAll();
			return View(items.ToList()[0]);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
