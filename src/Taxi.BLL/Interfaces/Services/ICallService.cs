using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface ICallService // : IService<CallDto>
	{
		Task<int> GetCountAsync(int tariffId, DateTime day, int driverId, int mechanicId);
		Task<IEnumerable<CallDto>> GetAllAsync(int tariffId, DateTime day, int driverId, int mechanicId, int skip, int take);
		IEnumerable<AddressDto> GetPopularAddresses(int numberMonth);
	}
}
