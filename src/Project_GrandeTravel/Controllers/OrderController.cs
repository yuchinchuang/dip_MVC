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

namespace Project_GrandeTravel.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private UserManager<MyUser> _userManagerService;
        private IRepository<Order> _orderRepo;
        private IRepository<Package> _packRepo;
        private IRepository<Feedback> _feedbackRepo;
        private IRepository<MyUser> _userRepo;
        private IRepository<ProviderProfile> _provRepo;

        public OrderController(UserManager<MyUser> userManager, IRepository<Order> orderRepo, 
            IRepository<Package> packRepo, IRepository<Feedback> feedbackRepo, IRepository<MyUser> userRepo, IRepository<ProviderProfile> provRepo)
        {
            _userManagerService = userManager;
            _orderRepo = orderRepo;
            _packRepo = packRepo;
            _feedbackRepo = feedbackRepo;
            _userRepo = userRepo;
            _provRepo = provRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            CreateOrderViewModel vm = new CreateOrderViewModel
            {
                UserId = loggedUser.Id,
                Package = _packRepo.GetSingle(p => p.PackageId == id),
                PackageId = id
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel vm, int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {

                Order order = new Order
                {
                    PackageId = vm.PackageId,
                    UserId = loggedUser.Id,
                    Email = vm.Email,
                    Mobile = vm.Mobile,
                    OrderDate = DateTime.Now,
                    DepartingDate = vm.DepartingDate,
                    NumberOfAdult = vm.NumberOfAdult,
                    Comment = vm.Comment,
                    IsActive = true
                };

                _orderRepo.Create(order);

                return RedirectToAction("Index", "Customer", new { id = loggedUser.Id});
            }

            vm.PackageId = id;
            vm.Package = _packRepo.GetSingle(p => p.PackageId == id);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Display(int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);
            Order order = _orderRepo.GetSingle(o => o.OrderId == id);
            Package pack = _packRepo.GetSingle(p => p.PackageId == order.PackageId);
            ProviderProfile provider = _provRepo.GetSingle(pv => pv.UserId == pack.UserId);

            DisplaySingleOrderViewModel vm = new DisplaySingleOrderViewModel
            {
                OrderId = id,
                Package = pack,
                Email = order.Email,
                Mobile = order.Mobile,
                NumberOfAdult = order.NumberOfAdult,
                OrderDate = order.OrderDate,
                DepartingDate = order.DepartingDate,
                Comment = order.Comment,
                Provider = provider
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            Order order = _orderRepo.GetSingle(o => o.OrderId == id);
            Package pack = _packRepo.GetSingle(p => p.PackageId == order.PackageId);

            if (order.DepartingDate < DateTime.Today)
            {
                order.IsActive = false;
            }

            if (order != null)
            {
                if (order.IsActive == true)
                {
                    ManageOrderViewModel vm = new ManageOrderViewModel
                    {
                        OrderId = id,
                        Email = order.Email,
                        Mobile = order.Mobile,
                        DepartingDate = order.DepartingDate,
                        OrderDate = order.OrderDate,
                        NumberOfAdult = order.NumberOfAdult,
                        IsActive = order.IsActive,
                        Comment = order.Comment,
                        Package = pack
                    };
                    return View(vm);
                }

                ViewBag.Message = "Cannot manage: this booking is expired/ cancelled.";
                return RedirectToAction("Display", "Order", new { id = id });

            }

            return RedirectToAction("Display", "Order", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(int id, ManageOrderViewModel vm)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            Order order = _orderRepo.GetSingle(o => o.OrderId == id);
            Package pack = _packRepo.GetSingle(p => p.PackageId == order.PackageId);

            if (ModelState.IsValid && order != null)
            {
                order.Email = vm.Email;
                order.Mobile = vm.Mobile;
                order.DepartingDate = vm.DepartingDate;
                order.NumberOfAdult = vm.NumberOfAdult;
                order.Comment = vm.Comment;

                _orderRepo.Update(order);

                vm.Package = pack;

                return RedirectToAction("Display", "Order", new { id = id });
            }

            vm.PackageId = id;
            vm.Package = _packRepo.GetSingle(p => p.PackageId == id);

            ModelState.AddModelError("", "Information required!");

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeedback(CreateFeedbackViewModel vm, int id)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

                Feedback feedback = new Feedback
                {
                    Content = vm.FeedbackContent,
                    OrderId = vm.OrderId,
                    PackageId = vm.PackageId
                };

                _feedbackRepo.Create(feedback);

                return RedirectToAction("Display", "Package", new { id = vm.PackageId });
            }

            return RedirectToAction("Index","Customer");
        }
        
    }
}
