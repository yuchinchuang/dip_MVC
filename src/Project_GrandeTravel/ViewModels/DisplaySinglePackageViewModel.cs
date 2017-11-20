using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplaySinglePackageViewModel
    {
        public int PackId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }

        public string CreatorUserName { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public int TotalF { get; set; }

        public string ImgPath { get; set; }

        public string MapUrl { get; set; }
    }
}
