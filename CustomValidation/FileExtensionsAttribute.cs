using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomValidation
{
  public  class FileExtensionsAttribute :ValidationAttribute
    {
      public override bool IsValid(object value)
      {
          if (value == null)
              return true;

          var file = value as HttpPostedFileBase;
          string ext = System.IO.Path.GetExtension(file.FileName).ToLower();

          string[] validExt = { ".png", ".jpg", ".jpeg", ".gif" };

          return validExt.Contains(ext);
      }
    }
}
