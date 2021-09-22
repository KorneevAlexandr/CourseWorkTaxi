using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface ITariffService : IService<TariffDto>
	{
		Task<IEnumerable<TariffDto>> GetAllAsync();
		Task<TariffDto> GetAsync(int id);
	}
}
