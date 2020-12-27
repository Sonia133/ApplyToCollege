using College.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace College.CustomValidations
{
    public class CnpValidator : ValidationAttribute
    {
        private bool BeUnique(string cnp, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Students.FirstOrDefault(st => st.Cnp.ToString() == cnp && !(st.StudentId == id)) == null;
        }

        private ValidationResult validateCNP(double cnp, int id)
        {
            var cnpString = cnp.ToString();

            if (cnpString == null)
                return new ValidationResult("CNP field is required!");

            Regex regex = new Regex(@"^([1-9]\d+)$");
            if (!regex.IsMatch(cnpString))
                return new ValidationResult("CNP must contain only digits!");

            if (cnpString.Length != 13)
                return new ValidationResult("CNP must contain 13 digits!");

            if (!BeUnique(cnpString, id))
                return new ValidationResult("CNP must be unique!");

            
            return ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var student = (Student)validationContext.ObjectInstance;
            double cnp = student.Cnp;
            int id = student.StudentId;

            return validateCNP(cnp, id);
        }
    }
}