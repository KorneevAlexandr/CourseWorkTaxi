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
		
		Task<EmployeeDto> GetRandomAsync(int positionId);
	}
}
