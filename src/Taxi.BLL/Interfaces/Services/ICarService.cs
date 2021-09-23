using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface ICarService
	{
		Task<IEnumerable<CarDto>> GetAllAsync(int brandId, int mileage, int price, int issueYear, int skip, int take);
		Task<int> GetCountAsync(int brandId, int mileage, int price, int issueYear);
		IEnumerable<CarDto> GetAllForTI();
	}
}
