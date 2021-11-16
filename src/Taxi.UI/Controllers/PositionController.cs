using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Filters;
using Taxi.UI.Models.Positions;

namespace Taxi.UI.Controllers
{
	public class PositionController : Controller
	{
		private readonly IPositionService _positionService;

		public PositionController(IPositionService positionService)
		{
			_positionService = positionService;
		}

		public async Task<IActionResult> Index()
		{
			var positions = await _positionService.GetAllAsync();
			var model = positions.Select(x => new PositionViewModel
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
			});
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(PositionViewModel model)
		{
			var position = new PositionDto
			{
				Name = model.Name,
				Description = model.Description,
			};
			await _positionService.CreateAsync(position);
			return RedirectToAction("Index");
		}

		[DeleteExceptionFilter]
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			await _positionService.DeleteAsync(id.Value);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var position = await _positionService.GetAsync(id.Value);
			var model = new PositionViewModel
			{
				Id = position.Id,
				Name = position.Name,
				Description = position.Description,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(PositionViewModel model)
		{
			var position = new PositionDto 
			{
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
			};
			await _positionService.UpdateAsync(position);
			return RedirectToAction("Index");
		}
	}
}
