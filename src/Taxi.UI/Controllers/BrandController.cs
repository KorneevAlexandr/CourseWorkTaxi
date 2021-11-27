using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Filters;
using Taxi.UI.Models.Brands;

namespace Taxi.UI.Controllers
{
	public class BrandController : Controller
	{
		private readonly IBrandService _brandService;

		public BrandController(IBrandService brandService)
		{
			_brandService = brandService;
		}

		public async Task<IActionResult> Index()
		{
			var items = await _brandService.GetAllAsync();
			var model = items.Select(x => new BrandViewModel
			{
				Id = x.Id,
				Name = x.Name,
			});
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(BrandViewModel model)
		{
			var item = new BrandDto
			{
				Name = model.Name,
			};
			await _brandService.CreateAsync(item);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			var brand = await _brandService.GetAsync(id.Value);
			var model = new BrandViewModel
			{
				Id = brand.Id,
				Name = brand.Name,
			};
			return View(model);
		}

		[DeleteExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> DeleteBrand(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			await _brandService.DeleteAsync(id.Value);
			return RedirectToAction("Index");
		}
		
		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			var item = await _brandService.GetAsync(id.Value);
			var model = new BrandViewModel
			{
				Id = item.Id,
				Name = item.Name,
			};
			return View(model);
		}

		public async Task<IActionResult> Update(BrandViewModel model)
		{
			var item = new BrandDto
			{
				Id = model.Id,
				Name = model.Name,
			};

			await _brandService.UpdateAsync(item);
			return RedirectToAction("Index");
		}

	}
}
