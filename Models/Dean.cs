using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace College.Models
{
    public class Dean
    {
        [ForeignKey("Faculty")]
        public int DeanId { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        // one to one
        public virtual Faculty Faculty { get; set; }
    }
}