using College.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College.CustomValidations
{
    public class MultipleOf10 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int places = -1;

            if (validationContext.ObjectType.Name == "FacultyDean")
            {
                var faculty = (FacultyDean)validationContext.ObjectInstance;
                places = faculty.Places;
            }
            else
            {
                 var faculty = (Faculty)validationContext.ObjectInstance;
                 places = faculty.Places;
            }

            bool cond = true;

            if (places % 10 != 0)
            {
                cond = false;
            }

            return cond ? ValidationResult.Success : new ValidationResult("This is not a multiple of 10!");
        }
    }
}