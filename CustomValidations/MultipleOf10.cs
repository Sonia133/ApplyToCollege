using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College.CustomValidations
{
    public class MultipleOf10 : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int places = (int)value;

            return (places % 10 == 0);
        }
    }
}