using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenAPIViewModel
{
    //Give Users current details
    public class CurrentUserWebApiViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public CurrentUserViewModel data { get; set; }
    }

    public class UpdateUserWebApiModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public UpdateUserViewModel data { set; get; }
    }

    public class ChangePasswordWebApiViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public ChangePasswordViewModel data { get; set; }
    }

    public class RegisterWebApiViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public RegisterViewModel data { get; set; }
    }

    public class UserProfileWebApiViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public UserViewModel data { get; set; }
    }

    public class ForgotPasswdWebApiViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public OnlyEmailViewModel data { get; set; }
    }

}
