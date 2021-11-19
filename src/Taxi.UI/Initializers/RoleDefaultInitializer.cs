using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.UI.Data;

namespace Taxi.UI.Initializers
{
	public class RoleDefaultInitializer : IDefaultInitializer
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleDefaultInitializer(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task InitializeAsync()
		{
			var roles = new List<IdentityRole>
			{
				new IdentityRole(DefaultRoles.Admin.ToString()),
				new IdentityRole(DefaultRoles.Driver.ToString()),
				new IdentityRole(DefaultRoles.Dispatcher.ToString()),
				new IdentityRole(DefaultRoles.Employee.ToString()),
				new IdentityRole(DefaultRoles.CarManager.ToString()),
				new IdentityRole(DefaultRoles.PersonnelManager.ToString()),
			};

			var oldRole = _roleManager.Roles;
			foreach (var role in roles)
			{
				if (!oldRole.Contains(role))
				{
					await _roleManager.CreateAsync(role);
				}
			}
		}
	}
}
