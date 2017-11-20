using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplayAllCategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public int Total { get; set; }
    }
}
