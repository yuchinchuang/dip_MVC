using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;
using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class CreateFeedbackViewModel
    {
        [Required]
        public string FeedbackContent { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int PackageId { get; set; }
    }
}
