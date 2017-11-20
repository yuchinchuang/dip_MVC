using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;
using Project_GrandeTravel.ViewModels;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Hosting; 
using System.IO; 

namespace Project_GrandeTravel.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private IRepository<Category> _catRepo;
        private IRepository<Package> _packageRepo;
        private IHostingEnvironment _environment;

        public CategoryController(IRepository<Category> repo, IRepository<Package> packRepo, IHostingEnvironment environment)
        {
            _catRepo = repo;
            _packageRepo = packRepo;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Category> catList = _catRepo.GetAll();
            DisplayAllCategoriesViewModel vm = new DisplayAllCategoriesViewModel
            {
                Categories = catList,
                Total = catList.Count()
            };
            return View(vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Display(int id, string sortOrder)
        {
            Category cat = _catRepo.GetSingle(c => c.CategoryId == id);

            IEnumerable<Package> thePackages = _packageRepo.Query(p => p.CategoryId == id);

            thePackages = thePackages.Where(pA => pA.IsActive == true);

            ViewData["LocationSortParm"] = string.IsNullOrEmpty(sortOrder) ? "location_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";

            switch (sortOrder)
            {
                case "location_desc":
                    thePackages = thePackages.OrderByDescending(p => p.Location);
                    break;
                case "Price":
                    thePackages = thePackages.OrderBy(p => p.Price);
                    break;
                case "Price_desc":
                    thePackages = thePackages.OrderByDescending(p => p.Price);
                    break;
                default:
                    thePackages = thePackages.OrderBy(p => p.Location);
                    break;
            }

            DisplaySingleCategoryViewModel vm = new DisplaySingleCategoryViewModel
            {
                CatId = cat.CategoryId,
                CatName = cat.Name,
                CatDescription = cat.Description,
                CatPackages = thePackages,
                TotalPackage = thePackages.Count(),
                ImgPath = cat.ImgPath
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCategoryViewModel vm, IFormFile ImgPath)
        {
            if (ModelState.IsValid)
            {
                Category cat = new Category
                {
                    Name = vm.Name,
                    Description = vm.Description
                };

                string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                Directory.CreateDirectory(Path.Combine(uploadPath, cat.Name));// path = Uploads/UserName/CatName/

                if(ImgPath != null)
                {
                    string fileName = Path.GetFileName(ImgPath.FileName);

                    using(FileStream fs = new FileStream(Path.Combine(uploadPath, cat.Name, fileName), FileMode.Create))
                    {
                        ImgPath.CopyTo(fs);
                    }

                    cat.ImgPath = Path.Combine(User.Identity.Name, cat.Name, fileName);
                }
                else
                {
                    cat.ImgPath = ""; 
                }

                _catRepo.Create(cat);

                return RedirectToAction("Index", "Category");
            }

            return View(vm);

        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            Category cat = _catRepo.GetSingle(c => c.CategoryId == id);

            if (cat != null)
            {
                UpdateCategoryViewModel vm = new UpdateCategoryViewModel
                {
                    CatId = id,
                    Name = cat.Name,
                    Description = cat.Description,
                    ImgPath = cat.ImgPath
                };
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, UpdateCategoryViewModel vm, IFormFile ImgPath)
        {
            Category cat = _catRepo.GetSingle(c => c.CategoryId == id);

            if (ModelState.IsValid && cat != null)
            {
                cat.Name = vm.Name;
                cat.Description = vm.Description;

                if (ImgPath != null)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");
                    uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                    Directory.CreateDirectory(Path.Combine(uploadPath, cat.Name));


                    string fileName = Path.GetFileName(ImgPath.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, cat.Name, fileName), FileMode.Create))
                    {
                        ImgPath.CopyTo(fs);
                    }

                    cat.ImgPath = Path.Combine(User.Identity.Name, cat.Name, fileName);
                }

                _catRepo.Update(cat);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Information required!");

            return View(vm);
        }
    }
}
