using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface ICallRepository : IRepository<Call>
	{
		Task<IQueryable<Call>> GetAllAsync(int tariffId, DateTime? day, int driverId, int dispatherId, int skip, int take);
		
		Task<int> GetCountAsync(int tariffId, DateTime? day, int driverId, int dispatherId);

		Task<Dictionary<string, int>> GetPopularStartStreet(int year, int month);

		Task<Dictionary<string, int>> GetPopularEndStreet(int year, int month);
	}
}
