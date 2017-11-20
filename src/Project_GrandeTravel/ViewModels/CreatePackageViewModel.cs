using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Project_GrandeTravel.Models;

namespace Project_GrandeTravel.ViewModels
{
    public class CreatePackageViewModel
    {
        
        public IEnumerable<Category> CatList { get; set; }
        [Required,
            Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public bool IsActive { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Photo")]
        public string ImgPath { get; set; }

    }
}
