using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;
using Project_GrandeTravel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace Project_GrandeTravel.Controllers
{
    [Authorize(Roles = "Provider")]
    public class PackageController : Controller
    {
        private IRepository<Category> _catRepo;
        private IRepository<Package> _packRepo;
        private IRepository<MyUser> _userRepo;
        private IRepository<Feedback> _feedbackRepo;
        private IRepository<Order> _ordRepo;
        private UserManager<MyUser> _userManagerService;
        private IHostingEnvironment _environment;

        public PackageController(IRepository<Category> catkRepo, IRepository<Package> packRepo, UserManager<MyUser> userManagerService, IRepository<MyUser> userRepo,
            IRepository<Order> ordRepo, IRepository<Feedback> feedbackRepo, IHostingEnvironment environment)
        {
            _catRepo = catkRepo;
            _packRepo = packRepo;
            _userManagerService = userManagerService;
            _userRepo = userRepo;
            _feedbackRepo = feedbackRepo;
            _ordRepo = ordRepo;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Display(int id)
        {
            Package pack = _packRepo.GetSingle(p => p.PackageId == id);
            MyUser creator = _userRepo.GetSingle(u => u.Id == pack.UserId);
            IEnumerable<Feedback> feedbacks = _feedbackRepo.Query(f => f.PackageId == id);

            string apiUrl = "https://www.google.com/maps/embed/v1/place?key=";
            string apiKey = "";
            string query = "&q=";
            System.Text.StringBuilder locationBuilder = new System.Text.StringBuilder(pack.Location);

            for(int i = 0; i < pack.Location.Length; i++)
            {
                if(locationBuilder[i] == ' ')
                {
                    locationBuilder[i] = '+';
                }
            }

            DisplaySinglePackageViewModel vm = new DisplaySinglePackageViewModel
            {
                ImgPath = pack.ImgPath,
                PackId = pack.PackageId,
                Name = pack.Name,
                Location = pack.Location,
                Description = pack.Description,
                Price = pack.Price,
                IsActive = pack.IsActive,
                CreatorUserName = creator.UserName,
                MapUrl = apiUrl + apiKey + query + locationBuilder.ToString(),
                Feedbacks = feedbacks,
                TotalF = feedbacks.Count()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            CreatePackageViewModel vm = new CreatePackageViewModel
            {
                UserId = loggedUser.Id,
                CatList = _catRepo.GetAll()
            };

            if(id != 0)
            {
                vm.CategoryId = id;
            }

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePackageViewModel vm, IFormFile ImgPath)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                Package p = new Package
                {
                    CategoryId = vm.CategoryId,
                    Name = vm.Name,
                    Location = vm.Location,
                    Description = vm.Description,
                    Price = vm.Price,
                    IsActive = vm.IsActive,
                    UserId = loggedUser.Id
                };

                string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                Directory.CreateDirectory(Path.Combine(uploadPath, p.Name));

                if (ImgPath != null)
                {                    
                    string fileName = Path.GetFileName(ImgPath.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, p.Name, fileName), FileMode.Create))
                    {
                        ImgPath.CopyTo(fs);
                    }

                    p.ImgPath = Path.Combine(User.Identity.Name, p.Name, fileName);
                }
                else
                {
                    p.ImgPath = "";
                }

                _packRepo.Create(p);

                return RedirectToAction("Index", "Provider", new { id = vm.UserId });
            }

            vm.CatList = _catRepo.GetAll();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            Package pack = _packRepo.GetSingle(p => p.PackageId == id);

            if (pack != null)
            {
                UpdatePackageViewModel vm = new UpdatePackageViewModel
                {
                    PackId = id,
                    Name = pack.Name,
                    Location = pack.Location,
                    Description = pack.Description,
                    Price = pack.Price,
                    IsActive = pack.IsActive,
                    UserId = loggedUser.Id,
                    ImgPath = pack.ImgPath
                };
                return View(vm);
            }

            return RedirectToAction("Index", "Category");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdatePackageViewModel vm, IFormFile ImgPath)
        {
            var loggedUser = await _userManagerService.FindByNameAsync(User.Identity.Name);

            Package pack = _packRepo.GetSingle(p => p.PackageId == id);

            if (ModelState.IsValid && pack != null)
            {
                pack.Name = vm.Name;
                pack.Location = vm.Location;
                pack.Description = vm.Description;
                pack.Price = vm.Price;
                pack.IsActive = vm.IsActive;
                pack.UserId = loggedUser.Id;

                if (ImgPath != null)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                    uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                    Directory.CreateDirectory(Path.Combine(uploadPath, pack.Name));

                    string fileName = Path.GetFileName(ImgPath.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, pack.Name, fileName), FileMode.Create))
                    {
                        ImgPath.CopyTo(fs);
                    }

                    pack.ImgPath = Path.Combine(User.Identity.Name, pack.Name, fileName);
                }

                _packRepo.Update(pack);

                return RedirectToAction("Display", "Package", new { id = pack.PackageId });

            }

            ModelState.AddModelError("", "Information required!");

            return View(vm);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string keyword, string location, string sortOrder)
        {
            string searchLocation = "";
            IEnumerable<Package> packList = _packRepo.Query(p => p.IsActive == true);
            
            string searchKeyword = "";

            SearchPackageViewModel vm = new SearchPackageViewModel();

            if (!(string.IsNullOrEmpty(location) || location.Equals("All")))
            {
                searchLocation = location;
                packList = packList.Where(p => p.Location.ToUpper().Contains(location.ToUpper()));
            }
            else
            {
                searchLocation = "All";
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                string input = keyword.ToUpper();
                searchKeyword = keyword;
                packList = packList.Where(pk => pk.Name.ToUpper().Contains(input) || pk.Description.ToUpper().Contains(input));
            }

            ViewData["LocationSortParm"] = string.IsNullOrEmpty(sortOrder) ? "location_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";

            switch (sortOrder)
            {
                case "location_desc":
                    packList = packList.OrderByDescending(p => p.Location);
                    break;
                case "Price":
                    packList = packList.OrderBy(p => p.Price);
                    break;
                case "Price_desc":
                    packList = packList.OrderByDescending(p => p.Price);
                    break;
                default:
                    packList = packList.OrderBy(p => p.Location);
                    break;
            }

            vm.SearchLocation = searchLocation;
            vm.Keyword = searchKeyword;
            vm.Packages = packList;
            vm.Total = packList.Count();

            return View(vm);
        }
    }
}
