using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class UpdatePackageViewModel
    {
        public int PackId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required,
            DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Change Photo")]
        public string ImgPath { get; set; }

    }
}
