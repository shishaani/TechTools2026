using Microsoft.AspNetCore.Identity;
 
namespace TechTools.Services
{
    public class RoleService(RoleManager<IdentityRole> roleManager)
    {
        public async Task<List<IdentityRole>> GetRoles()
        {
            return roleManager.Roles.ToList();
        }
 
        public async Task<bool> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return false;
 
            if (await roleManager.RoleExistsAsync(roleName)) return false;
 
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }
    }
}