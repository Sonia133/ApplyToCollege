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
            if (id > -1)
            {
                return db.Faculties.FirstOrDefault(fac => fac.Name == name && !(fac.FacultyId == id)) == null;
            }
            return db.Faculties.FirstOrDefault(fac => fac.Name == name) == null;
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
            string name = "";
            int id = -1;

            if (validationContext.ObjectType.Name == "FacultyDean")
            {
                var facultyDean = (FacultyDean)validationContext.ObjectInstance;
                name = facultyDean.Name;

                return validateName(name, id);
            }
            else
            {
                var faculty = (Faculty)validationContext.ObjectInstance;
                name = faculty.Name;
                id = faculty.FacultyId;

                return validateName(name, id);
            }
        }
    }
}