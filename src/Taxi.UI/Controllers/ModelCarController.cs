using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Filters;
using Taxi.UI.Models.Brands;
using Taxi.UI.Models.CarModels;

namespace Taxi.UI.Controllers
{
	public class ModelCarController : Controller
	{
		private const int AMOUNT = 5;

		private readonly IBrandService _brandService;
		private readonly IModelService _modelService;
		private int _currentPage;

		public ModelCarController(
			IBrandService brandService, 
			IModelService modelService)
		{
			_brandService = brandService;
			_modelService = modelService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? id, int? page)
		{
			var selectedId = id == null ? 0 : id.Value;
			_currentPage = page == null ? 0 : page.Value - 1;

			ModelCollectionViewModel model;
			var brands = await _brandService.GetAllAsync();
			var modelBrands = brands.Select(x => new BrandViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();
			modelBrands.Insert(0, new BrandViewModel { Id = 0, Name = "Любая" });

			var models = await _modelService.GetAllAsync(selectedId, AMOUNT * _currentPage, AMOUNT);
			var countModels = await _modelService.GetCountAsync(selectedId);
			model = new ModelCollectionViewModel
			{
				CountPages = countModels % AMOUNT == 0
					? (int)(countModels / AMOUNT) : (int)(countModels / AMOUNT) + 1,
				CurrentPage = _currentPage + 1,
				Id = selectedId,
				Brands = modelBrands,
				Models = models.Select(x => new ModelViewModel
				{
					Id = x.Id,
					Body = x.Body,
					Fuel = x.Fuel,
					HP = x.HP,
					Name = x.Name,
					Price = x.Price,
				}).ToList(),
			};
		
		return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var brands = await _brandService.GetAllAsync();
			var modelBrands = brands.Select(x => new BrandViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();
			var model = new ModelViewModel
			{
				Bodys = BodysData.Bodys,
				Fuels = FuelsData.Fuels,
				Brands = modelBrands,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ModelViewModel model)
		{
			var modelDto = new ModelDto
			{
				Name = model.Name,
				Body = BodysData.GetBodyName(model.BodyId),
				Fuel = FuelsData.GetFuelName(model.FuelId),
				HP = model.HP,
				Price = model.Price,
				BrandId = model.BrandId,
			};

			await _modelService.CreateAsync(modelDto);
			return Redirect($"~/ModelCar/Index?Id={model.BrandId}");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			var modelCar = await _modelService.GetAsync(id.Value);
			var model = new ModelViewModel
			{
				Id = modelCar.Id,
				Body = modelCar.Body,
				Fuel = modelCar.Fuel,
				HP = modelCar.HP,
				Name = modelCar.Name,
				Price = modelCar.Price,
			};
			return View(model);
		}

		[DeleteExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> DeleteModelCar(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var model = await _modelService.GetAsync(id.Value);
			await _modelService.DeleteAsync(id.Value);
			return Redirect($"~/ModelCar/Index?Id={model.BrandId}");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			var brands = await _brandService.GetAllAsync();
			var model = await _modelService.GetAsync(id.Value);
			var modelView = new ModelViewModel
			{
				Id = model.Id,
				Name = model.Name,
				BodyId = BodysData.GetBodyId(model.Body),
				Bodys = BodysData.Bodys,
				FuelId = FuelsData.GetFuelId(model.Fuel),
				Fuels = FuelsData.Fuels,
				HP = model.HP,
				Price = model.Price,
				BrandId = model.BrandId,
				Brands = brands.Select(x => new BrandViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList(),
			};
			return View(modelView);
		}

		[HttpPost]
		public async Task<IActionResult> Update(ModelViewModel model)
		{
			var modelDto = new ModelDto
			{
				Id = model.Id,
				Name = model.Name,
				Body = BodysData.GetBodyName(model.BodyId),
				Fuel = FuelsData.GetFuelName(model.FuelId),
				HP = model.HP,
				Price = model.Price,
				BrandId = model.BrandId,
			};
			await _modelService.UpdateAsync(modelDto);
			return Redirect($"~/ModelCar/Index?Id={model.BrandId}");
		}
	}
}
