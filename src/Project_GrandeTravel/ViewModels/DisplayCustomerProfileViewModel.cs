using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class DisplayCustomerProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<Order> ActiveOrders { get; set; }
        public IEnumerable<Order> InactiveOrders { get; set; }
        public IEnumerable<Package> Packages { get; set; }

        public string ImgPath { get; set; }
    }
}
