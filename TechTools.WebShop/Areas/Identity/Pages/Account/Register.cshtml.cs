using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TechTools.WebShop.Areas.Identity.Pages.Account
{
    public class RegisterModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<RegisterModel> logger) : PageModel
    {
        private const string DefaultCustomerRole = "klant";

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "{0} moet minstens {2} en maximum {1} tekens lang zijn.", MinimumLength = 4)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Het wachtwoord en bevestigingswachtwoord komen niet overeen.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Input.Email,
                Email = Input.Email
            };

            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");

                await EnsureCustomerRoleExists();
                await userManager.AddToRoleAsync(user, DefaultCustomerRole);

                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private async Task EnsureCustomerRoleExists()
        {
            if (!await roleManager.RoleExistsAsync(DefaultCustomerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(DefaultCustomerRole));
            }
        }
    }
}
