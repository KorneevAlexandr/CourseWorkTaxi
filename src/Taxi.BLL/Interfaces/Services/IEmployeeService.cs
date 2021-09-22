using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.ModelsDto;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IEmployeeService : IService<EmployeeDto>
	{
		Task<IEnumerable<EmployeeDto>> GetAll(int skip, int take);
	}
}
