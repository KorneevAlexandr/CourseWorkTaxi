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
		Task<ModelDto> GetAsync(int id);
	}
}
