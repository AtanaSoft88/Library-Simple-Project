﻿using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        

        public UserController(UserManager<ApplicationUser> _userManager,
                            SignInManager<ApplicationUser> _signInManager)
                           

        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated??false)
            {
                return RedirectToAction("All", "Books");
            }
            var model = new RegisterViewModel();
            return this.View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {          

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
            };

            
            var result = await userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "User");
                
            }

            
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return this.View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Books");
            }
            var model = new LoginViewModel();
            return this.View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {                    
                var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("All", "Books");
                }

            }
            ModelState.AddModelError("", "Invalid login");
            return this.View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); 
        }
    }
}