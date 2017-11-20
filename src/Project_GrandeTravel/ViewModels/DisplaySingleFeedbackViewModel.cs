using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplaySingleFeedbackViewModel
    {
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public string CustomerName { get; set; }
        public string Content { get; set; }
    }
}
