using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.UI.Data
{
	public class IdentityTaxiContext : IdentityDbContext<User>
	{
		public IdentityTaxiContext(DbContextOptions<IdentityTaxiContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);
			modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
			modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => x.UserId);
		}

		}
	}
