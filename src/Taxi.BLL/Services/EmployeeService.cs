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
	internal class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly ICompleteRepository<Position> _positionRepository;

		public EmployeeService(IEmployeeRepository employeeRepository, ICompleteRepository<Position> positionRepository)
		{
			_employeeRepository = employeeRepository;
			_positionRepository = positionRepository;
		}

		public async Task<IEnumerable<EmployeeDto>> GetAllAsync(int skip, int take)
		{
			var items = await _employeeRepository.GetAllAsync(skip, take);
			return await ConvertCollection(items);
		}

		public async Task<IEnumerable<EmployeeDto>> GetAllAsync(int yearStanding, int positionId, int skip, int take)
		{
			if (yearStanding == 0 && positionId == 0)
			{
				return await GetAllAsync(skip, take);
			}
			else if (positionId == 0)
			{
				return await GetAllByStandingAsync(yearStanding, skip, take);
			}
			else if (yearStanding == 0)
			{
				return await GetAllByPositionAsync(positionId, skip, take);
			}

			var date = DateTime.Now;
			var validDate = new DateTime(date.Year - yearStanding, date.Month, date.Day);
			Func<IQueryable<Employee>, IQueryable<Employee>> func =
				(IQueryable<Employee> source) => source.Where(x =>
				x.DateStartOfWork <= validDate
				&& x.PositionId == positionId).Skip(skip).Take(take);

			var items = await _employeeRepository.GetAllAsync(func);
			return await ConvertCollection(items);
		}

		private async Task<IEnumerable<EmployeeDto>> GetAllByPositionAsync(int positionId, int skip, int take)
		{
			Func<IQueryable<Employee>, IQueryable<Employee>> func = (IQueryable<Employee> source)
				=> source.Where(x => x.PositionId == positionId).Skip(skip).Take(take);

			var items = await _employeeRepository.GetAllAsync(func);
			return await ConvertCollection(items);
		}

		private async Task<IEnumerable<EmployeeDto>> GetAllByStandingAsync(int yearStanding, int skip, int take)
		{
			var date = DateTime.Now;
			var validDate = new DateTime(date.Year - yearStanding, date.Month, date.Day);
			Func<IQueryable<Employee>, IQueryable<Employee>> func =
				(IQueryable<Employee> source) => source.Where(x =>
				x.DateStartOfWork <= validDate).Skip(skip).Take(take);

			var items = await _employeeRepository.GetAllAsync(func);
			return await ConvertCollection(items);
		}

		public async Task<int> GetCountAsync(int positionId, int yearStanding)
		{
			Func<IQueryable<Employee>, IQueryable<Employee>> func = null;
			if (positionId == 0 && yearStanding == 0)
			{
				func = (IQueryable<Employee> source) => source.Select(x => x);
			}
			else if (positionId == 0)
			{
				var date = DateTime.Now;
				var validDate = new DateTime(date.Year - yearStanding, date.Month, date.Day);
				func = (IQueryable<Employee> source) => source.Where(x =>
				x.DateStartOfWork <= validDate);
			}
			else // positionId != 0
			{
				func = (IQueryable<Employee> source)
				=> source.Where(x => x.PositionId == positionId);
			}

			return await _employeeRepository.GetCountAsync(func);
		}

		public async Task<EmployeeDto> GetAsync(int id)
		{
			var item = await _employeeRepository.GetAsync(id);
			var position = await _positionRepository.GetAsync(item.PositionId);
			var dtoItem = ItemConvert(item);
			dtoItem.PositionName = position.Name;
			return dtoItem;
		}

		public async Task<EmployeeDto> GetRandomAsync(int positionId)
		{
			var random = new Random();

			var countEmployees = await GetCountAsync(positionId, 0);
			var allEmployees = await GetAllByPositionAsync(positionId, 0, countEmployees);
			var randomEmployees = allEmployees.Skip(random.Next(0, countEmployees)).Take(1).FirstOrDefault();
			return randomEmployees;
		}

		public async Task CreateAsync(EmployeeDto entity)
		{
			await _employeeRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task DeleteAsync(int id)
		{
			await _employeeRepository.DeleteAsync(id);
		}

		public async Task UpdateAsync(EmployeeDto entity)
		{
			await _employeeRepository.UpdateAsync(ItemConvert(entity));
		}


		private async Task<IEnumerable<EmployeeDto>> ConvertCollection(IQueryable<Employee> items)
		{
			var employees = items.Select(x => ItemConvert(x));
			var listEmployees = employees.ToList();
			
			foreach (var item in listEmployees)
			{
				var position = await _positionRepository.GetAsync(item.PositionId);
				item.PositionName = position.Name;
			}

			return listEmployees;
		}

		private Employee ItemConvert(EmployeeDto employee)
		{
			return new Employee
			{
				Id = employee.Id,
				DateStartOfWork = employee.DateStartOfWork,
				Name = employee.Name,
				PositionId = employee.PositionId,
				Surname = employee.Surname,
			};
		}

		private EmployeeDto ItemConvert(Employee employee)
		{
			return new EmployeeDto
			{
				Id = employee.Id,
				DateStartOfWork = employee.DateStartOfWork,
				Name = employee.Name,
				PositionId = employee.PositionId,
				Surname = employee.Surname,
				//PositionName = employee.Position.Name,
			};
		}
	}
}
