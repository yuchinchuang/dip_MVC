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
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private UserManager<MyUser> _userManagerService;
        private IRepository<CustomerProfile> _profileRepo;
        private IRepository<Order> _orderRepo;
        private IRepository<Package> _packRepo;
        private IHostingEnvironment _environment;

        public CustomerController(UserManager<MyUser> userManager, IRepository<CustomerProfile> profileRepo,
            IRepository<Order> orderRepo, IRepository<Package> packRepo, IHostingEnvironment environment)
        {
            _userManagerService = userManager;
            _profileRepo = profileRepo;
            _orderRepo = orderRepo;
            _packRepo = packRepo;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            CustomerProfile loggedProfile = _profileRepo.GetSingle(cProfile => cProfile.UserId == loggedUser.Id);

            IEnumerable<Package> packList = _packRepo.GetAll();

            IEnumerable<Order> orderList = _orderRepo.Query(o => o.UserId == loggedUser.Id);
            List<Order> activeOrder = new List<Order>();
            List<Order> inactiveOrder = new List<Order>();

                foreach (var o in orderList)
                {
                    if (o.DepartingDate >= DateTime.Now)
                    {
                        o.IsActive = true;
                        activeOrder.Add(o);
                    }
                    else
                    {
                        o.IsActive = false;
                        inactiveOrder.Add(o);
                    }
                }


            DisplayCustomerProfileViewModel vm = new DisplayCustomerProfileViewModel
            {
                UserId = loggedUser.Id,
                UserName = loggedUser.UserName,
                Packages = packList,
                ActiveOrders = activeOrder,
                InactiveOrders = inactiveOrder
            };

            if(loggedProfile != null)
            {
                if (loggedProfile.ImgPath == null)
                {
                    vm.ImgPath = "admin\\person-solid.png";
                }
                else
                {
                    vm.ImgPath = loggedProfile.ImgPath;
                }
            }
            else
            {
                vm.ImgPath = "admin\\person-solid.png";
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            CustomerProfile loggedProfile = _profileRepo.GetSingle(cProfile => cProfile.UserId == loggedUser.Id);

            UpdateCustomerProfileViewModel vm = new UpdateCustomerProfileViewModel
            {
                UserId = loggedUser.Id,
                Email = loggedUser.Email,
                ImgPath = "admin\\person-solid.png"
            };

            if(loggedProfile != null)
            {
                vm.FirstName = loggedProfile.FirstName;
                vm.LastName = loggedProfile.LastName;
                vm.Email = loggedProfile.Email;
                vm.Mobile = loggedProfile.Mobile;

                if (loggedProfile.ImgPath != null)
                {
                    vm.ImgPath = loggedProfile.ImgPath;
                }

            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateCustomerProfileViewModel vm, IFormFile ImgPath)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

                CustomerProfile loggedProfile = _profileRepo.GetSingle(p => p.UserId == loggedUser.Id);

                string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                Directory.CreateDirectory(Path.Combine(uploadPath, "Profile"));

                if (ImgPath != null)
                {
                    string fileName = Path.GetFileName(ImgPath.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, "Profile", fileName), FileMode.Create))
                    {
                        ImgPath.CopyTo(fs);
                    }

                    vm.ImgPath = Path.Combine(User.Identity.Name, "Profile", fileName);
                }

                if (loggedProfile != null)
                {
                    loggedProfile.FirstName = vm.FirstName;
                    loggedProfile.LastName = vm.LastName;
                    loggedProfile.Email = vm.Email;
                    loggedProfile.Mobile = vm.Mobile;
                    loggedProfile.ImgPath = vm.ImgPath;

                    _profileRepo.Update(loggedProfile);
                }
                else
                {
                    loggedProfile = new CustomerProfile();

                    loggedProfile.FirstName = vm.FirstName;
                    loggedProfile.LastName = vm.LastName;
                    loggedProfile.Email = vm.Email;
                    loggedProfile.Mobile = vm.Mobile;
                    loggedProfile.UserId = loggedUser.Id;
                    loggedProfile.ImgPath = vm.ImgPath;

                    _profileRepo.Create(loggedProfile);
                }
            }

            return RedirectToAction("Index", "Customer");
        }
    }
}
