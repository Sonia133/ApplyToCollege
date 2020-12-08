using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Models
{
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }
        public string Name { get; set; }
        public double Cnp { get; set; }
        public string Frequency { get; set; }
        public string Email { get; set; }
        public double Sat { get; set; }
        public int Badge { get; set; }

        // one to many
        [Required]
        public virtual Faculty Faculty { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> FacultiesList { get; set;}
    }
}