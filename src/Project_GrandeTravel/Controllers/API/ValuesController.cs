using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//..
using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;

namespace Project_GrandeTravel.Controllers.API
{
    public class ValuesController : Controller
    {
        private IRepository<Category> _catRepo;
        private IRepository<Package> _packRepo;

        public ValuesController(IRepository<Category> catRepo, IRepository<Package> packRepo)
        {
            _catRepo = catRepo;
            _packRepo = packRepo;
        }

        [HttpGet("api/Categories")]
        public IActionResult GetAllCat()
        {
            var allCat = _catRepo.GetAll();

            return Json(allCat);
        }

        [HttpGet("api/Packages")]
        public IActionResult GetAllPack()
        {
            var allPack = _packRepo.GetAll();
            List<Package> activePack = new List<Package>();
            foreach(var item in allPack)
            {
                if(item.IsActive == true)
                {
                    activePack.Add(item);
                }
            }
            return Json(activePack);
        }

        [HttpGet("api/GetPackLocation")]
        public JsonResult GetPackByLocation(string location)
        {
            var packList = _packRepo.Query(p => p.IsActive == true);
            if (!string.IsNullOrEmpty(location))
            {
                packList = packList.Where(p => p.Location.ToUpper().Contains(location.ToUpper()));
            }
            return Json(packList);
        }

        [HttpGet("api/GetPackKeyword")]//url/api/GetPackKeyword?location=${searchL}&keyword=${keyword}
        public JsonResult FilterPackKeyword(string location, string keyword)
        {
            var packList = _packRepo.Query(p => p.IsActive == true);

            if (!string.IsNullOrEmpty(location))
            {
                packList = packList.Where(p => p.Location.ToUpper().Contains(location.ToUpper()));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                packList = packList.Where(p => p.Name.ToUpper().Contains(keyword.ToUpper())
                                                        || p.Description.ToUpper().Contains(keyword.ToUpper()));
            }

            return Json(packList);
        }
    }
}
