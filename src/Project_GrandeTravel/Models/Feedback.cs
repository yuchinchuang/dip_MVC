using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_GrandeTravel.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string Content { get; set; }

        //FK
        public int OrderId { get; set; }
        public Order Order { get; set; }

        //modify
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
