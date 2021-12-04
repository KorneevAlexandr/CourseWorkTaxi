using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Data;
using Taxi.UI.Filters;
using Taxi.UI.Models.Brands;
using Taxi.UI.Models.Tariffs;

namespace Taxi.UI.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TariffController : Controller
	{
		private readonly ITariffService _tariffService;

		public TariffController(ITariffService tariffService)
		{
			_tariffService = tariffService;
		}

		public async Task<IActionResult> Index()
		{
			var tariffs = await _tariffService.GetAllAsync();
			var model = tariffs.Select(x => new TariffViewModel
			{
				Id = x.Id,
				Name = x.Name,
				Price = x.Price,
				Description = x.Description,
			});
			return View(model);
		}

		[OperationExceptionFilter]
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			var tariff = await _tariffService.GetAsync(id.Value);
			var model = new TariffViewModel
			{
				Id = tariff.Id,
				Description = tariff.Description,
				Name = tariff.Name,
				Price = tariff.Price,
			};
			return View(model);
		}

		[OperationExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> DeleteTariff(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			await _tariffService.DeleteAsync(id.Value);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(TariffViewModel model)
		{
			var tariff = new TariffDto
			{
				Name = model.Name,
				Description = model.Description,
				Price = model.Price,
			};
			await _tariffService.CreateAsync(tariff);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var tariff = await _tariffService.GetAsync(id.Value);
			var model = new TariffViewModel
			{
				Id = tariff.Id,
				Name = tariff.Name,
				Description = tariff.Description,
				Price = tariff.Price,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(TariffViewModel model)
		{
			var tariff = new TariffDto
			{
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				Price = model.Price,
			};
			await _tariffService.UpdateAsync(tariff);
			return RedirectToAction("Index");
		}

	}
}
