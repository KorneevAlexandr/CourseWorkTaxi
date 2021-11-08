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
	internal class PositionService : IPositionService
	{
		private readonly ICompleteRepository<Position> _positionRepository;

		public PositionService(ICompleteRepository<Position> positionRepository)
		{
			_positionRepository = positionRepository;
		}

		public async Task<IEnumerable<PositionDto>> GetAllAsync()
		{
			var items = await _positionRepository.GetAllAsync();
			return items.Select(x => ItemConvert(x));

		}

		public async Task<PositionDto> GetAsync(int id)
		{
			var item = await _positionRepository.GetAsync(id);
			return ItemConvert(item);
		}

		public async Task CreateAsync(PositionDto entity)
		{
			await _positionRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task DeleteAsync(int id)
		{
			await _positionRepository.DeleteAsync(id);
		}

		public async Task UpdateAsync(PositionDto entity)
		{
			await _positionRepository.UpdateAsync(ItemConvert(entity));
		}

		private Position ItemConvert(PositionDto position)
		{
			return new Position
			{
				Id = position.Id,
				Name = position.Name,
				Description = position.Description,
			};
		}

		private PositionDto ItemConvert(Position position)
		{
			return new PositionDto
			{
				Id = position.Id,
				Description = position.Description,
				Name = position.Name,
			};
		}
	}
}
