using System;
using System.Collections.Generic;
using System.Globalization;
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
		private readonly IEmployeeRepository _employeeRepository;

		public CallService(ICallRepository callRepository, IEmployeeRepository employeeRepository)
		{
			_callRepository = callRepository;
			_employeeRepository = employeeRepository;
		}

		public async Task<IEnumerable<CallDto>> GetAllAsync(int tariffId, DateTime? day, int driverId, int dispatherId, int skip, int take)
		{
			var items = await _callRepository.GetAllAsync(tariffId, day, driverId, dispatherId, skip, take);
			return await CollectionConvert(items);
		}

		public async Task<int> GetCountAsync(int tariffId, DateTime? day, int driverId, int mechanicId)
		{
			return await _callRepository.GetCountAsync(tariffId, day, driverId, mechanicId);
		}

		public async Task CreateAsync(CallDto entity)
		{
			await _callRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task<CallDto> GetAsync(int id)
		{
			var call = await _callRepository.GetAsync(id);
			var callDto = await CollectionConvert(new List<Call> { call });
			return callDto.FirstOrDefault();
		}

		public async Task DeleteAsync(int id)
		{
			await _callRepository.DeleteAsync(id);
		}

		public async Task UpdateAsync(CallDto entity)
		{
			await _callRepository.UpdateAsync(ItemConvert(entity));
		}

		private async Task<IEnumerable<CallDto>> CollectionConvert(IEnumerable<Call> calls)
		{
			var items = new List<CallDto>();
			foreach (var item in calls.ToList())
			{
				var callDto = ItemConvert(item);
				var driver = await _employeeRepository.GetAsync(callDto.DriverId);
				var dispather = await _employeeRepository.GetAsync(callDto.DispatherId);
				
				callDto.DriverFullName = $"{driver.Surname} {driver.Name}";
				callDto.DispatherFullName = $"{dispather.Surname} {dispather.Name}";
				items.Add(callDto);
			}
			return items;
		}

		public async Task<IEnumerable<CallAddressDto>> GetPopularStartStreets()
		{
			var callAddresses = new List<CallAddressDto>();
			var now = DateTime.Now;
			for (int i = 1; i <= now.Month; i++)
			{
				var dictByMonth = await _callRepository.GetPopularStartStreet(now.Year, i);
				callAddresses.Add(new CallAddressDto
				{
					MonthNumber = i,
					AddressName = dictByMonth.FirstOrDefault().Key,
					CountCalls = dictByMonth.FirstOrDefault().Value,
				});
			}
			return callAddresses;
		}

		public async Task<IEnumerable<CallAddressDto>> GetPopularEndStreets()
		{
			var callAddresses = new List<CallAddressDto>();
			var now = DateTime.Now;
			for (int i = 1; i <= now.Month; i++)
			{
				var dictByMonth = await _callRepository.GetPopularEndStreet(now.Year, i);
				callAddresses.Add(new CallAddressDto
				{
					MonthNumber = i,
					AddressName = dictByMonth.FirstOrDefault().Key,
					CountCalls = dictByMonth.FirstOrDefault().Value,
				});
			}
			return callAddresses;
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
				Phone = call.Phone,
				Price = call.Price,
				StartStreet = call.StartStreet,
				EndStreet = call.EndStreet,
				StartHomeNumber = call.StartHomeNumber,
				EndHomeNumber = call.EndHomeNumber,
			};
		}

		private Call ItemConvert(CallDto call)
		{
			return new Call
			{
				Id = call.Id,
				DispatherId = call.DispatherId,
				CallDateTime = call.CallDateTime,
				CarId = call.CarId,
				Phone = call.Phone,
				Price = call.Price,
				StartStreet = call.StartStreet,
				EndStreet = call.EndStreet,
				StartHomeNumber = call.StartHomeNumber,
				EndHomeNumber = call.EndHomeNumber,
			};
		}
	}
}
