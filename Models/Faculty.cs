using College.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace College.Models
{
    public class Faculty
    {
        [Key]
        [Column("faculty_id")]
        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        [MultipleOf10(ErrorMessage ="The number of places should be multiple of 10.")]
        public int Places { get; set; }
        public string Description { get; set; }


        // one to one
        public virtual Dean Dean { get; set; }

        // many to one
        public virtual ICollection<Teacher> Teachers { get; set; }
        // many to one
        public virtual ICollection<Exam> Exam { get; set; }
        // many to one
        public virtual ICollection<Student> Students { get; set; }

    }
}