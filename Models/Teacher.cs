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
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }


        // one to many
        public virtual Faculty Faculty { get; set; }
    }
}