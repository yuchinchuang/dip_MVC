using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_GrandeTravel.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int PackageId { get; set; }

        //contact - might be different from profile
        public string Email { get; set; }
        public string Mobile { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime DepartingDate { get; set; }
        public string Comment { get; set; }
        public int NumberOfAdult { get; set; }
        public List<Feedback> Feedbacks { get; set; }

        //cancelled / expired
        public bool IsActive { get; set; }





    }
}
