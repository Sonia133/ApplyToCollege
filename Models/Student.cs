using System;
using College.CustomValidations;
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
        [Required]
        public string Name { get; set; }
        [CnpValidator]
        public double Cnp { get; set; }
        [Required]
        public string Frequency { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required, Range(400, 1600)]
        public double Sat { get; set; }
        [Required, UniqueBadge]
        public int Badge { get; set; }

        // many to many
        public virtual ICollection<Faculty> Faculties { get; set; }

        [NotMapped]
        public List<Checkbox> FacultiesList { get; set;}
    }
}