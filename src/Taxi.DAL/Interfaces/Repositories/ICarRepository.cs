using System;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface ICarRepository : IRepository<Car>
	{
		Task<IQueryable<Car>> GetAllAsync(int skip, int take);
		
		Task<int> GetCountAsync();
		
		Task<IQueryable<Car>> GetAllAsync(int brandId, int mileage, int price, int issueYear, int skip, int take);
		
		Task<int> GetCountAsync(int brandId, int mileage, int price, int issueYear);
		
		IQueryable<Car> GetAllForTI(DateTime dateStart, DateTime dateEnd);

		Task<Car> GetRandomCarByTariff(int tariffId);
	}
}
