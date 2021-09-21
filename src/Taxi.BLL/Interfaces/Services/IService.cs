using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IService<T>
	{
		Task<IEnumerable<T>> GetAll();

		Task<T> GetAsync(int id);

		Task CreateAsync(T entity);

		Task DeleteAsync(int id);

		Task UpdateAsync(T entity);
	}
}
