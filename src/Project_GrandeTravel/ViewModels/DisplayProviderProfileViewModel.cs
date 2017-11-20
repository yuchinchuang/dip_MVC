using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplayProviderProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<Package> activePackages { get; set; }
        public IEnumerable<Package> inactivePackages { get; set; }

        public string ImgPath { get; set; }
    }
}
