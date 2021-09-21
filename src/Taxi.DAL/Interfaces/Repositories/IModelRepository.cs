﻿using System.Linq;
using System.Threading.Tasks;
using Taxi.DAL.Domain;

namespace Taxi.DAL.Interfaces.Repositories
{
	public interface IModelRepository : IRepository<Model>
	{
		Task<IQueryable<Model>> GetAllByBrand(int brandId);
	}
}
