using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTools.Services;

namespace TechTools.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController(RoleService roleService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var roles = await roleService.GetRoles();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (await roleService.Create(roleName))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Role bestaat al of is leeg!");
            return View();
        }
    }
}
