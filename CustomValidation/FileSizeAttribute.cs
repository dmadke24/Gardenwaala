using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CustomValidation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        int _maxLenth;
        public FileSizeAttribute(int MaxLength = 2)
        {
            _maxLenth = MaxLength;
        }
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var file = value as HttpPostedFileBase;
            int MaxLength = 1048576 * _maxLenth;

            return (file.ContentLength <= MaxLength);

        }
    }
}
