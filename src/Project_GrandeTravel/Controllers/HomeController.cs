using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;

namespace Project_GrandeTravel.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Category> _catRepo;

        public HomeController(IRepository<Category> catRepo)
        {
            _catRepo = catRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> catList = _catRepo.GetAll();

            string imgMapUrl = "../Category/Display/";

            foreach(var c in catList)
            {
                if (c.Name.Equals("New South Wales"))
                {
                    ViewData["NSW"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Victoria"))
                {
                    ViewData["VIC"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("South Australia"))
                {
                    ViewData["SA"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Western Australia"))
                {
                    ViewData["WA"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Queensland"))
                {
                    ViewData["QLD"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Northern Territory"))
                {
                    ViewData["NT"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Tasmania"))
                {
                    ViewData["TAS"] = imgMapUrl + c.CategoryId;
                }
                if (c.Name.Equals("Australian Capital Territory"))
                {
                    ViewData["ACT"] = imgMapUrl + c.CategoryId;
                }
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Background"] = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae elementum diam. Nullam eget est id nibh semper fringilla. Vestibulum nisl felis, vestibulum non libero auctor, egestas varius nibh. Donec interdum ex sit amet leo consequat volutpat. Sed quis lorem velit. Morbi sagittis rutrum risus sed egestas. Vestibulum mattis tempor suscipit. Duis non leo sagittis, faucibus augue id, luctus arcu. Nullam pretium, urna vel ultricies dapibus, dui lectus eleifend nisl, quis ullamcorper nisl dolor scelerisque eros. Integer vitae lorem felis.";
            ViewData["Goal"] = "Nullam iaculis ut lectus ut ullamcorper. Suspendisse malesuada, orci non aliquet venenatis, nulla tellus tincidunt tellus, vitae laoreet libero est sit amet nisl. Vivamus neque magna, euismod ut purus rhoncus, iaculis eleifend nibh. Suspendisse eu orci auctor nulla feugiat tincidunt.Maecenas vel lorem at erat hendrerit ullamcorper vel sed eros. Duis massa elit, commodo sed condimentum quis, feugiat eget metus. Morbi faucibus bibendum ante eget mollis.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
