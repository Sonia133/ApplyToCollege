using College.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College.CustomValidations
{
    public class UniqueName: ValidationAttribute
    {
        private bool BeUnique(string name, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Faculties.FirstOrDefault(fac => fac.Name == name && !(fac.FacultyId == id)) == null;
        }

        private ValidationResult validateName(string name, int id)
        {
            if (name == null)
                return new ValidationResult("Name field is required!");

            if (!BeUnique(name, id))
                return new ValidationResult("Name must be unique!");


            return ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var faculty = (Faculty)validationContext.ObjectInstance;
            string name = faculty.Name;
            int id = faculty.FacultyId;

            return validateName(name, id);
        }
    }
}