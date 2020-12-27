using College.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College.CustomValidations
{
    public class UniqueBadge : ValidationAttribute
    {
        private bool BeUnique(int badge, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Students.FirstOrDefault(st => st.Badge == badge && !(st.StudentId == id)) == null;
        }

        private ValidationResult validateBadge(int badge, int id)
        {

            if (!BeUnique(badge, id))
                return new ValidationResult("Badge must be unique!");


            return ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var student = (Student)validationContext.ObjectInstance;
            int badge = student.Badge;
            int id = student.StudentId;

            return validateBadge(badge, id);
        }
    }
}