using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Exceptions;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;

namespace Taxi.BLL.Services
{
	internal class BrandService : IBrandService
	{
		private readonly ICompleteRepository<Brand> _brandRepository;

		public BrandService(ICompleteRepository<Brand> brandRepository)
		{
			_brandRepository = brandRepository;
		}

		public async Task<IEnumerable<BrandDto>> GetAllAsync()
		{
			var items = await _brandRepository.GetAllAsync();
			return items.Select(x => new BrandDto
			{
				Id = x.Id,
				Name = x.Name,
			});
		}

		public async Task<BrandDto> GetAsync(int id)
		{
			var item = await _brandRepository.GetAsync(id);
			return new BrandDto
			{
				Id = item.Id,
				Name = item.Name,
			};
		}

		public async Task CreateAsync(BrandDto entity)
		{
			var item = new Brand
			{
				Id = entity.Id,
				Name = entity.Name,
			};
			await _brandRepository.CreateAsync(item);
		}

		public async Task DeleteAsync(int id)
		{
			try
			{
				await _brandRepository.DeleteAsync(id);
			}
			catch
			{
				throw new InvalidDeleteOperationException("Нельзя удалить данные об этой марке автомобиля, так как от нее зависят другие данные. " +
					"Удалите или измените зависимые данные (модели автомобилей и сами автомобили) и повторите попытку.", "Марка автомобиля");
			}
		}

		public async Task UpdateAsync(BrandDto entity)
		{
			var item = new Brand
			{
				Id = entity.Id,
				Name = entity.Name,
			};
			await _brandRepository.UpdateAsync(item);
		}
	}
}
