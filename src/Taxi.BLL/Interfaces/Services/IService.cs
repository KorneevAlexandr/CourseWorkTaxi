﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.BLL.Interfaces.Services
{
	public interface IService<T>
	{
		Task CreateAsync(T entity);

		Task DeleteAsync(int id);

		Task UpdateAsync(T entity);
	}
}
