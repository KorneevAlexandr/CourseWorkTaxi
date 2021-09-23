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
	public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IPositionService _positionService;

		public EmployeeService(string connectionString)
		{
			_accountRepository = new AccountRepository(connectionString);
			_employeeRepository = new EmployeeRepository(connectionString);
			_positionService = new PositionService(connectionString);
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
				func = (IQueryable<Employee> source) => source.Where(x =>
				(DateTime.Now - x.DateStartOfWork).Days >= (yearStanding * 365));
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
			return ItemConvert(item);
		}

		/// <summary>
		/// Применять, предварительно проверив логин!
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		public async Task<EmployeeDto> GetAsync(string login)
		{
			var account = await _accountRepository.GetAsync(login);
			if (account == null)
			{
				return null;
			}
			var item = await _employeeRepository.GetAsync(account.EmployeeId);
			return ItemConvert(item);
		}

		public async Task<int> EmployeeIsAutorizeAsync(string login, string password)
		{
			var account = await _accountRepository.GetAsync(login);
			if (account.Password.Equals(password))
			{
				return account.EmployeeId;
			}
			return 0;
		}

		public async Task CreateAuthorizeAsync(EmployeeDto employee, string login, string pasword)
		{
			await _employeeRepository.CreateAsync(ItemConvert(employee));
			var item = await _employeeRepository.GetLastAsync();

			var account = new Account
			{
				Login = login,
				Password = pasword,
				EmployeeId = item.Id,
			};
			await _accountRepository.CreateAsync(account);
		}

		public async Task<bool> IsUniqueLoginAsync(string login)
		{
			var item = await _accountRepository.GetAsync(login);
			if (item == null)
			{
				return true;
			}
			return false;
		}

		public async Task CreateAsync(EmployeeDto entity)
		{
			await _employeeRepository.CreateAsync(ItemConvert(entity));
		}

		public async Task DeleteAsync(int id)
		{
			var item = await _accountRepository.GetByEmployeeAsync(id);
			await _accountRepository.DeleteAsync(item.EmployeeId);
			await _employeeRepository.DeleteAsync(id);
		}

		public async Task UpdateAsync(EmployeeDto entity)
		{
			await _employeeRepository.UpdateAsync(ItemConvert(entity));
		}

		public async Task UpdateAuthorizeAsync(int employeeId, string login, string password)
		{
			var item = await _accountRepository.GetByEmployeeAsync(employeeId);
			item.Login = login;
			item.Password = password;
			await _accountRepository.UpdateAsync(item);
		}

		private async Task<IQueryable<EmployeeDto>> ConvertCollection(IQueryable<Employee> items)
		{
			var employees = items.Select(x => ItemConvert(x));

			foreach (var item in employees)
			{
				var account = await _accountRepository.GetByEmployeeAsync(item.Id);
				var position = await _positionService.GetAsync(item.PositionId);
				if (account != null)
				{
					item.Login = account.Login;
				}
				item.PositionName = position.Name;
			}

			return employees;
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
			};
		}
	}
}
