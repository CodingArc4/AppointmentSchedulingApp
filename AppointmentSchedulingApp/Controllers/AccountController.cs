using AppointmentSchedulingApp.Models;
using AppointmentSchedulingApp.Models.ViewModels;
using AppointmentSchedulingApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AppointmentSchedulingApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            if(!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Patient));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
            }
            return View();
        }

        //POST Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    Name = registerViewModel.Name,
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerViewModel.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }
    }
}
