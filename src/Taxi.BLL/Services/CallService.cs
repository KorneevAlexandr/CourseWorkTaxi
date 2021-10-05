﻿using System;
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
		private readonly IEmployeeRepository _employeeRepository;

		public CallService(string connectonString)
		{
			_callRepository = new CallRepository(connectonString);
			_employeeRepository = new EmployeeRepository(connectonString);
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
