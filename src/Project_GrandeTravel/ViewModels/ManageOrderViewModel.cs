using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project_GrandeTravel.Models;

using System.ComponentModel.DataAnnotations;
namespace Project_GrandeTravel.ViewModels
{
    public class ManageOrderViewModel
    {
        public int OrderId { get; set; }

        [Required,
            DataType(DataType.EmailAddress),
            RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        public DateTime OrderDate { get; set; }

        [Required,
            DataType(DataType.Date), Display(Name = "Departing Date"),
            DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime DepartingDate { get; set; }

        [Required,
            Display(Name = "Adult")]
        public int NumberOfAdult { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public bool IsActive { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
