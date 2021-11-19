using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IModelService : IService<ModelDto>
	{
		Task<IEnumerable<ModelDto>> GetAllByBrandAsync(int id);

		Task<IEnumerable<ModelDto>> GetAllAsync(int brandId, int skip, int take);

		Task<int> GetCountAsync(int brandId);

		Task<ModelDto> GetAsync(int id);
	}
}
