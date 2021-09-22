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
	public class TariffService : ITariffService
	{
		private readonly ICompleteRepository<Tariff> _tariffRepository;

		public TariffService(string connectionString)
		{
			_tariffRepository = new TariffRepository(connectionString);
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
			await _tariffRepository.DeleteAsync(id);
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
