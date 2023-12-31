﻿using AppointmentSchedulingApp.Models;
using AppointmentSchedulingApp.Models.ViewModels;
using AppointmentSchedulingApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
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

        //POST Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                    loginViewModel.RememberMe,false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginViewModel.Email);
                    HttpContext.Session.SetString("ssuserName", user.Name);
                    //var userName = HttpContext.Session.GetString("ssuserName");
                    return RedirectToAction("Index", "Appointment");
                }
                ModelState.AddModelError("", "Invalid Login attempt!");
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Register()
        {
            //if roles doesnt exists then create roles
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

                var result = await _userManager.CreateAsync(user,registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerViewModel.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["newAdminSignUp"] = user.Name;
                    }
                    return RedirectToAction("Index", "Appointment");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            } 
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }
}
