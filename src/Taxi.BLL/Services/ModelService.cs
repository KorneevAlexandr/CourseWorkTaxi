using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Exceptions;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.BLL.Services
{
	internal class ModelService : IModelService
	{
		private readonly IModelRepository _modelRepository;

		public ModelService(IModelRepository modelRepository)
		{
			_modelRepository = modelRepository;
		}

		public async Task<IEnumerable<ModelDto>> GetAllByBrandAsync(int id)
		{
			var items = await _modelRepository.GetAllByBrandAsync(id);
			return items.Select(x => ItemConvert(x));
		}

		public async Task<ModelDto> GetAsync(int id)
		{
			var item = await _modelRepository.GetAsync(id);
			return ItemConvert(item);
		}

		public async Task CreateAsync(ModelDto entity)
		{
			await _modelRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task DeleteAsync(int id)
		{
			try
			{
				await _modelRepository.DeleteAsync(id);
			}
			catch
			{
				throw new InvalidDeleteOperationException("Нельзя удалить данные об этой модели автомобиля, так как от нее зависят данные об автомобилях." +
					"Удалите или измените зависимые данные (автомобили) и повторите попытку.", "Модель автомобиля");
			}
		}

		public async Task UpdateAsync(ModelDto entity)
		{
			await _modelRepository.UpdateAsync(ItemConvert(entity));
		}

		private ModelDto ItemConvert(Model model)
		{
			return new ModelDto
			{
				Id = model.Id,
				Name = model.Name,
				Body = model.Body,
				BrandId = model.BrandId,
				Fuel = model.Fuel,
				HP = model.HP,
				Price = model.Price,
				BrandName = model.Brand.Name,
			};
		}

		private Model ItemConvert(ModelDto model)
		{
			return new Model
			{
				Id = model.Id,
				Name = model.Name,
				Body = model.Body,
				BrandId = model.BrandId,
				Fuel = model.Fuel,
				HP = model.HP,
				Price = model.Price,
			};
		}
	}
}
