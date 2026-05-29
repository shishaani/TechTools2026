using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechTools.Models;

namespace SportsClub.WebApp.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<IdentityUser> userManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }


        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var vm = new UserRolesVM
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = new List<RoleCheckbox>()
            };

            foreach (var role in _roleManager.Roles)
            {
                vm.Roles.Add(new RoleCheckbox
                {
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRolesVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);

            foreach (var role in vm.Roles)
            {
                if (role.IsSelected && !await _userManager.IsInRoleAsync(user, role.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else if (!role.IsSelected && await _userManager.IsInRoleAsync(user, role.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }

            return RedirectToAction("Index");
        }
    }
}