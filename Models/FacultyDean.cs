using College.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College.Models
{
    public class FacultyDean
    {
        [UniqueName]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required, MultipleOf10]
        public int Places { get; set; }
        [Required,
         MaxLength(200, ErrorMessage = "Please write a shorter description.")]
        public string Description { get; set; }

        [Required]
        public string DeanName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}