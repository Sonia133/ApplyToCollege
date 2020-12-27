using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace College.Models
{
    public class Teacher
    {
        [Key]
        [Column("teacher_id")]
        public int TeacherId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,
         MaxLength(20, ErrorMessage = "Subject name too long.")]
        public string Subject { get; set; }
        [EmailAddress]
        public string Email { get; set; }


        // one to many
        public virtual Faculty Faculty { get; set; }
    }
}