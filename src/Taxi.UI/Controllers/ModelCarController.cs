using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Models.Brands;
using Taxi.UI.Models.CarModels;

namespace Taxi.UI.Controllers
{
	public class ModelCarController : Controller
	{
		private readonly IBrandService _brandService;
		private readonly IModelService _modelService;

		public ModelCarController(
			IBrandService brandService, 
			IModelService modelService)
		{
			_brandService = brandService;
			_modelService = modelService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? Id)
		{
			ModelCollectionViewModel model;
			var brands = await _brandService.GetAllAsync();
			var modelBrands = brands.Select(x => new BrandViewModel
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();

			if (Id == null)
			{
				model = new ModelCollectionViewModel
				{
					Models = new List<ModelViewModel>(),
					Brands = modelBrands,
				};
			}
			else
			{
				var models = await _modelService.GetAllByBrandAsync(Id.Value);
				model = new ModelCollectionViewModel
				{
					Id = Id.Value,
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
			}

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
				Body = model.Body,
				Fuel = model.Fuel,
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
				Body = model.Body,
				Fuel = model.Fuel,
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
				Body = model.Body,
				Fuel = model.Fuel,
				HP = model.HP,
				Price = model.Price,
				BrandId = model.BrandId,
			};
			await _modelService.UpdateAsync(modelDto);
			return Redirect($"~/ModelCar/Index?Id={model.BrandId}");
		}
	}
}
