using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IModelRepository : IRepository<Model>
	{
		Task<IQueryable<Model>> GetAllByBrandAsync(int brandId);

		Task<IQueryable<Model>> GetAllAsync(int brandId, int skip, int take);

		Task<int> GetCountAsync(int brandId);
	}
}
