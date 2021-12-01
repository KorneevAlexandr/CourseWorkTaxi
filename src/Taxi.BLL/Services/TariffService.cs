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
	internal class TariffService : ITariffService
	{
		private readonly ICompleteRepository<Tariff> _tariffRepository;

		public TariffService(ICompleteRepository<Tariff> tariffRepository)
		{
			_tariffRepository = tariffRepository;
		}

		public async Task<IEnumerable<TariffDto>> GetAllAsync()
		{
			var items = await _tariffRepository.GetAllAsync();
			return items.Select(x => ItemConvert(x));
		}

		public async Task<TariffDto> GetAsync(int id)
		{
			var item = await _tariffRepository.GetAsync(id);
			return ItemConvert(item);
		}

		public async Task CreateAsync(TariffDto entity)
		{
			await _tariffRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task DeleteAsync(int id)
		{
			try
			{
				await _tariffRepository.DeleteAsync(id);
			}
			catch
			{
				throw new InvalidDeleteOperationException("Нельзя удалить данные об этом тарифе, так как от него зависят данные об автомобилях и вызовах." +
					"Удалите или измените зависимые данные (автомобили и вызовы) и повторите попытку.", "Тариф");
			}
		}

		public async Task UpdateAsync(TariffDto entity)
		{
			await _tariffRepository.UpdateAsync(ItemConvert(entity));
		}

		private TariffDto ItemConvert(Tariff tariff)
		{
			return new TariffDto
			{
				Id = tariff.Id,
				Name = tariff.Name,
				Description = tariff.Description,
				Price = tariff.Price,
			};
		}

		private Tariff ItemConvert(TariffDto tariff)
		{
			return new Tariff
			{
				Id = tariff.Id,
				Name = tariff.Name,
				Description = tariff.Description,
				Price = tariff.Price,
			};
		}
	}
}
