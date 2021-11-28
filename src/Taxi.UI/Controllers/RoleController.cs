using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taxi.BLL.Interfaces.Services;
using Taxi.BLL.ModelsDto;
using Taxi.UI.Data;
using Taxi.UI.Filters;
using Taxi.UI.Models.Accounts;
using Taxi.UI.Models.Users;

namespace Taxi.UI.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private readonly IEmployeeService _employeeService;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
            IEmployeeService employeeService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _employeeService = employeeService;
        }

        [ResponseCache(CacheProfileName = "DefaultCache")]
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRolePost(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users); 
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var employee = await _employeeService.GetAsync(user.EmployeeId);
            var role = User.Claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).FirstOrDefault().Value;

            var model = new UserViewModel
            {
                UserId = userId,
                RoleName = role,
                Email = user.Email,
                Name = employee.Name,
                Surname = employee.Surname,
                StartWork = employee.DateStartOfWork,
                PositionName = employee.PositionName,
            };

            return View(model);
        }

        [OperationExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> DeleteUserPost(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.UserName.Equals(User.Identity.Name))
                {
                    throw new InvalidOperationException("Нельзя удалить текущего пользователя.");
                }
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, string role)
        {
            // получаем пользователя
            var user = await _userManager.FindByIdAsync(userId);
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(us => us.Email.Equals(User.Identity.Name));
            if (user != null)
            {
                if (currentUser.Equals(user))
                {
                    return RedirectToAction("UserList");
                }
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.AddToRoleAsync(user, role);
                await _userManager.RemoveFromRoleAsync(user, userRoles.FirstOrDefault());

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
