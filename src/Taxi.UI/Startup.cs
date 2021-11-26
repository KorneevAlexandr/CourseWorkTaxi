using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.BLL.Services;
using Taxi.UI.Data;
using Taxi.BLL.Extensions;
using Taxi.UI.Diagnostics;
using Taxi.UI.Settings;
using Taxi.DataInitialization.Extensions;
using Taxi.DataInitialization.Options;
using Taxi.UI.Initializers;

namespace Taxi.UI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			services.InjectServices(connectionString);

			services.AddDbContext<IdentityTaxiContext>(options =>
				options.UseSqlServer(connectionString));
			services.AddIdentity<User, IdentityRole>(opts =>
			{
				opts.Password.RequiredLength = 5;   // минимальная длина
				opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
				opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
				opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
				opts.Password.RequireDigit = false; // требуются ли цифры
			}).AddEntityFrameworkStores<IdentityTaxiContext>();

			services.AddScoped<IDefaultInitializer, RoleDefaultInitializer>();
			services.AddScoped<IDefaultInitializer, PositionDefaultInitializer>();

			var initializeSettings = Configuration.GetSection(nameof(DataInitializeSettings));
			services.InjectInitializers(new InitializeOptions
			{
				Initialize = initializeSettings.GetValue<bool>(nameof(DataInitializeSettings.DataInitialize)),
				CountCars = initializeSettings.GetValue<int>(nameof(DataInitializeSettings.CountCars)),
				CountCalls = initializeSettings.GetValue<int>(nameof(DataInitializeSettings.CountCalls)),
				CountEmployees = initializeSettings.GetValue<int>(nameof(DataInitializeSettings.CountEmployees)),
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
					options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseDefaultDataInitilize();
			app.UseDataInitialize();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseRequestSerilog();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
