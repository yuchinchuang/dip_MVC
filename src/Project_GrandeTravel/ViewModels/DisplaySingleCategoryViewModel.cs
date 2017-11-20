using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplaySingleCategoryViewModel
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string CatDescription { get; set; }
        public IEnumerable<Package> CatPackages { get; set; }
        public int TotalPackage { get; set; }

        public string ImgPath { get; set; }
    }
}
