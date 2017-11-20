using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_GrandeTravel.Models
{
    public class ProviderProfile
    {
        public int ProviderProfileId { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        //addImg
        public string ImgPath { get; set; }

    }
}
