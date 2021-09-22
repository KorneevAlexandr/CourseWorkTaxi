using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IBrandService : IService<BrandDto>
	{
		Task<IEnumerable<BrandDto>> GetAllAsync();
		Task<BrandDto> GetAsync(int id);
	}
}
