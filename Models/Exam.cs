using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace College.Models
{
    public class Exam
    {
        [Key]
        [Column("exam_id")]
        public int ExamId { get; set; }
        [Required,
         MaxLength(20, ErrorMessage = "Subject name too long.")]
        public string Subject { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Type { get; set; }


        // one to many
        public virtual Faculty Faculty { get; set; }
    }
}