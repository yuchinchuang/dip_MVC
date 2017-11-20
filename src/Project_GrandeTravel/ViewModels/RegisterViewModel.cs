using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Project_GrandeTravel.ViewModels
{
    public class RegisterViewModel
    {
        [Required,MaxLength(256)]
        public string Username { get; set; }

        [Required,
            DataType(DataType.EmailAddress),
            RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,
            DataType(DataType.Password),
            Compare(nameof(Password)),
            Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
