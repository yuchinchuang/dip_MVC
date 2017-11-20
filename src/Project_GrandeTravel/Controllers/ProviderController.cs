using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;
using Project_GrandeTravel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Project_GrandeTravel.Controllers
{
    [Authorize(Roles = "Provider")]
    public class ProviderController : Controller
    {
        private UserManager<MyUser> _userManagerService;
        private IRepository<ProviderProfile> _profileRepo;
        private IRepository<Package> _packRepo;
        private IHostingEnvironment _environment;

        public ProviderController(UserManager<MyUser> userManager, IRepository<ProviderProfile> profileRepo, 
            IRepository<Package> packRepo, IHostingEnvironment environment)
        {
            _userManagerService = userManager;
            _profileRepo = profileRepo;
            _packRepo = packRepo;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            ProviderProfile loggedProfile = _profileRepo.GetSingle(p => p.UserId == loggedUser.Id);

            if(loggedProfile == null)
            {
                return RedirectToAction("UpdateProfile");
            }

            DisplayProviderProfileViewModel vm = new DisplayProviderProfileViewModel
            {
                UserId = loggedUser.Id,
                UserName = loggedUser.UserName,
                activePackages = _packRepo.Query(p => p.UserId == loggedUser.Id && p.IsActive == true),
                inactivePackages = _packRepo.Query(p => p.UserId == loggedUser.Id && p.IsActive == false),
            };

            if(loggedProfile.ImgPath == null)
            {
                vm.ImgPath = "admin\\person-solid.png";
            }
            else
            {
                vm.ImgPath = loggedProfile.ImgPath;
            }


            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            ProviderProfile loggedProfile = _profileRepo.GetSingle(p => p.UserId == loggedUser.Id);

            UpdateProviderProfileViewModel vm = new UpdateProviderProfileViewModel
            {
                UserId = loggedUser.Id,
                Email = loggedUser.Email,
                ImgPath = "admin\\person-solid.png"
            };

            if(loggedProfile != null)
            {
                vm.CompanyName = loggedProfile.CompanyName;
                vm.Phone = loggedProfile.Phone;
                vm.Address = loggedProfile.Address;

                if(loggedProfile.ImgPath != null)
                {
                    vm.ImgPath = loggedProfile.ImgPath;
                }
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProviderProfileViewModel vm, IFormFile ImgPath)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

                ProviderProfile loggedProfile = _profileRepo.GetSingle(p => p.UserId == loggedUser.Id);

                string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                Directory.CreateDirectory(Path.Combine(uploadPath, "Profile"));


                if(loggedProfile != null)
                {
                    loggedProfile.CompanyName = vm.CompanyName;
                    loggedProfile.Email = vm.Email;
                    loggedProfile.Phone = vm.Phone;
                    loggedProfile.Address = vm.Address;

                    if (ImgPath != null)
                    {
                        string fileName = Path.GetFileName(ImgPath.FileName);

                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, "Profile", fileName), FileMode.Create))
                        {
                            ImgPath.CopyTo(fs);
                        }

                        loggedProfile.ImgPath = Path.Combine(User.Identity.Name, "Profile", fileName);
                    }
                 

                    _profileRepo.Update(loggedProfile);
                }
                else
                {
                    loggedProfile = new ProviderProfile();

                    loggedProfile.CompanyName = vm.CompanyName;
                    loggedProfile.Email = vm.Email;
                    loggedProfile.Phone = vm.Phone;
                    loggedProfile.Address = vm.Address;
                    loggedProfile.UserId = loggedUser.Id;

                    if (ImgPath != null)
                    {
                        string fileName = Path.GetFileName(ImgPath.FileName);

                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, "Profile", fileName), FileMode.Create))
                        {
                            ImgPath.CopyTo(fs);
                        }

                        loggedProfile.ImgPath = Path.Combine(User.Identity.Name, "Profile", fileName);
                    }

                    _profileRepo.Create(loggedProfile);
                }
            }
            ViewData["Message"] = "Login successfully";
            return RedirectToAction("Index", "Provider");
        }
    }
}
