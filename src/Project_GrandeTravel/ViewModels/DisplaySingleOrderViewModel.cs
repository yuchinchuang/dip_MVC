using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;
using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplaySingleOrderViewModel
    {
        public int OrderId { get; set; }
        public Package Package { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DepartingDate { get; set; }
        public int NumberOfAdult { get; set; }
        public string Comment { get; set; }
        public ProviderProfile Provider { get; set; }
    }
}
