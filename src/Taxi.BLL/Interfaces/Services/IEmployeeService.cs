using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IEmployeeService : IService<EmployeeDto>
	{
		Task<IEnumerable<EmployeeDto>> GetAllAsync(int skip, int take);
		
		Task<IEnumerable<EmployeeDto>> GetAllAsync(int yearStanding, int positionId, int skip, int take);
		
		Task<int> GetCountAsync(int positionId, int yearStanding);
		
		Task<EmployeeDto> GetAsync(int id);
		
		Task<EmployeeDto> GetAsync(string login);

		Task<EmployeeDto> GetRandomAsync(int positionId);

		/// <summary>
		/// Return EmployeeId, if he is authorize
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		Task<int> EmployeeIsAutorizeAsync(string login, string password);
		
		Task CreateAuthorizeAsync(EmployeeDto employee, string login, string pasword);
		
		Task<bool> IsUniqueLoginAsync(string login);
		
		Task UpdateAuthorizeAsync(int employeeId, string login, string password);
	}
}
