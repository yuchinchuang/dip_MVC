using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class UpdateProviderProfileViewModel
    {
        public string UserId { get; set; }

        [Required,
            Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Required,
            DataType(DataType.EmailAddress),
            RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required,
            DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Display(Name = "Change Photo")]
        public string ImgPath { get; set; }
    }
}
