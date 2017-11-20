using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;
using Project_GrandeTravel.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Project_GrandeTravel.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<MyUser> _userManagerService;
        private SignInManager<MyUser> _signInManagerService;

        public AccountController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            _userManagerService = userManager;
            _signInManagerService = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                MyUser user = new MyUser { UserName = vm.Username };
                var result = await _userManagerService.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    await _userManagerService.SetEmailAsync(user, vm.Email);
                    await _userManagerService.AddToRoleAsync(user, vm.Role);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {

            LoginViewModel vm = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManagerService.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, false);
                var loggedUser = await _userManagerService.FindByNameAsync(vm.Username);

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                    else
                    {
                        GrandeTravelDbContext db = new GrandeTravelDbContext();

                        var roleProvider = (from r in db.Roles where r.Name.Contains("Provider") select r).FirstOrDefault();
                        var roleCustomer = (from r in db.Roles where r.Name.Contains("Customer") select r).FirstOrDefault();

                        var usersProvider = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleProvider.Id)).ToList();
                        var usersCustomer = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleCustomer.Id)).ToList();


                        if (usersProvider.Find(p => p.Id == loggedUser.Id) != null)
                        {
                            return RedirectToAction("Index", "Provider");
                        }
                        else if (usersCustomer.Find(c => c.Id == loggedUser.Id) != null)
                        {
                            return RedirectToAction("Index", "Customer");
                        }

                    }
                }
            }

            ModelState.AddModelError("", "Username or Password incorrect!");

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManagerService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
 