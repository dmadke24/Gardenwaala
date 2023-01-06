using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomValidation
{
    public class IsGreater : ValidationAttribute
    {
        public string  OtherDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropInfo = validationContext.ObjectType.GetProperty(OtherDate);
            if(otherPropInfo==null)
            {
                return new ValidationResult("Could not find " + OtherDate);
            }
            object otherPropValue = otherPropInfo.GetValue(validationContext.ObjectInstance);

            string[] fromDtStr = Convert.ToString(otherPropValue).Split('-');
            string[] toDtStr = Convert.ToString(value).Split('-');

            DateTime fromDt = new DateTime(Convert.ToInt32(fromDtStr[2]), Convert.ToInt32(fromDtStr[1]), Convert.ToInt32(fromDtStr[0]));
            DateTime toDt = new DateTime(Convert.ToInt32(toDtStr[2]), Convert.ToInt32(toDtStr[1]), Convert.ToInt32(toDtStr[0]));

            if (toDt.Date > fromDt.Date)
                return null;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
