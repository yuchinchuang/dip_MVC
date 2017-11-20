using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;
using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class SearchPackageViewModel
    {
        public string SearchLocation { get; set; }
        public IEnumerable<Package> Packages { get; set; }
        public int Total { get; set; }

        public string Keyword { get; set; }

    }
}
