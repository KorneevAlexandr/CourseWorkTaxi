using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.BLL.Services
{
	public class ModelService : IModelService
	{
		private readonly IModelRepository _modelRepository;

		public ModelService(string connectionString)
		{
			_modelRepository = new ModelRepository(connectionString);
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
			await _modelRepository.DeleteAsync(id);
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
