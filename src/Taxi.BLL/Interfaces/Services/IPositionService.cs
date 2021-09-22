using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IPositionService : IService<PositionDto>
	{
		Task<IEnumerable<PositionDto>> GetAllAsync();
		Task<PositionDto> GetAsync(int id);
	}
}
