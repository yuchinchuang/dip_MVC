using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Photo")]
        public string ImgPath { get; set; }
    }
}
