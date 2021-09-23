using System;
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
	public class CallService : ICallService
	{
		private readonly ICallRepository _callRepository;
		private readonly ICompleteRepository<Address> _addressRepository;

		public CallService(string connectonString)
		{
			_callRepository = new CallRepository(connectonString);
			_addressRepository = new AddressRepository(connectonString);
		}

		public Task<IEnumerable<CallDto>> GetAllAsync(int tariffId, DateTime day, int driverId, int mechanicId, int skip, int take)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetCountAsync(int tariffId, DateTime day, int driverId, int mechanicId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<AddressDto> GetPopularAddresses(int numberMonth)
		{
			throw new NotImplementedException();
		}

		private CallDto ItemConvert(Call call)
		{
			return new CallDto
			{
				Id = call.Id,
				DispatherId = call.DispatherId,
				DriverId = call.Car.DriverId,
				CallDateTime = call.CallDateTime,
				CarId = call.CarId,
				EndAddress = $"{call.EndAddress.District}, {call.EndAddress.Street}, {call.EndAddress.HomeNumber}",
				StartAddress = $"{call.StartAddress.District}, {call.StartAddress.Street}, {call.StartAddress.HomeNumber}",
				Phone = call.Phone,
				Price = call.Price,
			};
		}
	}
}
