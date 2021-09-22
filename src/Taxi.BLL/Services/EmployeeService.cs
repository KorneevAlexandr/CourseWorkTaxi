using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.BLL.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IAccountRepository _accountRepository;

		public EmployeeService(string connectionString)
		{
			_accountRepository = new AccountRepository(connectionString);
			_employeeRepository = new EmployeeRepository(connectionString);
		}

		public async Task<IEnumerable<EmployeeDto>> GetAll(int skip, int take)
		{
			var items = await _employeeRepository.GetAllAsync(skip, take);
			var employees = items.Select(x => new EmployeeDto
			{
				Id = x.Id,
				Name = x.Name,
				Surname = x.Surname,
				DateStartOfWork = x.DateStartOfWork,
				PositionId = x.PositionId,
			}).ToList();

			foreach (var item in employees)
			{
				var account = await _accountRepository.GetByEmployee(item.Id);
				if (account != null)
				{
					item.Login = account.Login;
				}
			}

			return employees;
		}

		public Task CreateAsync(EmployeeDto entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}


		public Task UpdateAsync(EmployeeDto entity)
		{
			throw new NotImplementedException();
		}
	}
}
