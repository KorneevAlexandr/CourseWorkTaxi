using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.DAL.Domain;
using Taxi.DAL.Interfaces.Repositories;
using Taxi.DAL.Repositories;

namespace Taxi.BLL.Services
{
	public class BrandService : IService<BrandDto>
	{
		private readonly IBrandRepository _brandRepository;
		public BrandService(string connectionString)
		{
			_brandRepository = new BrandRepository(connectionString);
		}

		public async Task<IEnumerable<BrandDto>> GetAll()
		{
			var items = await _brandRepository.GetAllAsync();
			return items.Select(x => new BrandDto
			{
				Id = x.Id,
				Name = x.Name,
			});
		}

		public Task CreateAsync(BrandDto entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}



		public Task<BrandDto> GetAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(BrandDto entity)
		{
			throw new NotImplementedException();
		}
	}
}
