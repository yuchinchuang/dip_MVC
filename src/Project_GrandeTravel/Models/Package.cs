using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_GrandeTravel.Models
{
    public class Package
    {
        //ini
        public int PackageId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string UserId { get; set; }

        public string ImgPath { get; set; }

        //modify
        public List<Feedback> Feedbacks { get; set; }
    }
}
