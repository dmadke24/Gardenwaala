using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data;
using GardenViewModel;

namespace BAL
{
    public interface IUserRepository
    {
        CurrentUserViewModel Validate(string username, string password);//for USER
        CurrentUserViewModel ValidateEmail(string username, string password, string userEmail); //if user is ADMIN
        UserViewModel GetById(int uid);
        UpdateUserViewModel GetUserById(int uid); // for front end

        bool Update(UpdateUserViewModel model);
        bool UpdateUserProfile(UpdateUserViewModel model);
        string GetNameAndContactByEmail(string email);
        bool ChangePassword(ChangePasswordViewModel model);
        bool ResetPassword(string email, string newpassword);
        bool IsDuplicate(string email);
        bool Register(RegisterViewModel model);
        bool RegisterUser(RegisterViewModel model);
        UserGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        bool Delete(int id);
        string GetUserAddr(int uid);
        int GetUserCnt();
    }

    public class UserRepository : IUserRepository
    {
        MyDBContext db = new MyDBContext();


        private int NewId()
        {
            int oldid = db.Users.Select(b => b.UserId).DefaultIfEmpty().Max();
            int myid, newid;
            if (oldid == 0)
            {
                myid = 1;
                return myid;
            }
            else
            {
                int old_no = oldid;
                old_no += 1;
                newid = old_no;

                return (newid);
            }
        }

        public CurrentUserViewModel Validate(string username, string password)
        {
            User uobj = db.Users.Where(u => u.Email == username && u.Password == password && u.Status == true).FirstOrDefault();
            CurrentUserViewModel model = null;
            if (uobj != null)
            {
                model = new CurrentUserViewModel();
                model.UserId = uobj.UserId;
                model.Name = uobj.Name;
                model.Email = uobj.Email;
                model.Image = uobj.Image;
                model.Address = uobj.Address;
                model.ContactNo = uobj.ContactNo;
                db.SaveChanges();
            }

            return model;
        }

        //Validate Admin
        public CurrentUserViewModel ValidateEmail(string username, string password, string userEmail)
        {
            int cnt = db.Users.Where(u => u.Email == username && u.Password == password).Count();
            CurrentUserViewModel model = null;
            if (cnt == 1)
            {
                User uobj = db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
                if (uobj != null)
                {
                    model = new CurrentUserViewModel();
                    model.UserId = uobj.UserId;
                    model.Name = uobj.Name;

                    model.LastLogin = uobj.LastLogin;
                    model.Image = uobj.Image;

                    model.Email = username;
                    model.Password = password;
                    model.Role = uobj.Role.Name;
                }
            }
            return model;
        }

        public UserViewModel GetById(int uid) //Get user detils to my account
        {
            try
            {
                UserViewModel model = new UserViewModel();
                User obj = db.Users.Find(uid);
                if (obj != null)
                {
                    model.UserId = obj.UserId;
                    model.Image = obj.Image;
                    model.Name = obj.Name;
                    model.Address = obj.Address;
                    model.ContactNo = obj.ContactNo;
                    model.Gender = obj.Gender;
                    model.Email = obj.Email;
                    model.Status = obj.Status;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Update(UpdateUserViewModel model)
        {
            try
            {
                User obj = db.Users.Find(model.UserId);
                if (obj != null)
                {
                    obj.UserId = model.UserId;
                    obj.Name = model.Name;
                    obj.ContactNo = model.ContactNo;
                    obj.Gender = model.Gender;
                    obj.Address = model.Address;
                    obj.Image = model.Image;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
        }

        public string GetNameAndContactByEmail(string email)
        {
            string usernameContact = db.Database.SqlQuery<string>("Select Name + '|' + ContactNo From Users Where Email='" + email + "'").FirstOrDefault();
            return usernameContact;
        }

        public bool ChangePassword(ChangePasswordViewModel model)
        {
            // string encOldPass = Crypto.SHA1(model.OldPassword);
            User obj = db.Users.Where(u => u.Email == model.Username && u.Password == model.OldPassword).FirstOrDefault();
            if (obj != null)
            {
                obj.Password = model.NewPassword;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool ResetPassword(string email, string newpassword) //for forgot password
        {
            User obj = db.Users.Where(u => u.Email == email).FirstOrDefault();
            if (obj != null)
            {
                obj.Password = newpassword;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }


        public bool IsDuplicate(string email)
        {
            bool flag = db.Users.Where(u => u.Email == email).Count() > 0;
            return flag;
        }

        public bool Register(RegisterViewModel model)
        {
            try
            {
                User uobj = new User();
                uobj.UserId = NewId();
                uobj.Name = model.Name;
                uobj.Email = model.Email;
                uobj.ContactNo = model.ContactNo;
                uobj.Password = model.Password;
                uobj.CreatedDt = model.CreatedDt;
                uobj.RoleId = 2;
                uobj.UserTypeId = 2;

                uobj.Status = true;

                db.Users.Add(uobj);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
        }

        public CurrentUserViewModel SuperValidate(string username, string password)
        {
            throw new NotImplementedException();
        }


        public UserGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            UserGridViewModel gridModel = new UserGridViewModel();

            IQueryable<User> lstUser = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstUser = db.Users.Where(u => u.UserTypeId != 1);
            }
            else
            {
                lstUser = db.Users.Where(u => u.UserTypeId != 1 && u.Name.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "name":
                    lstUser = sortDir == "asc" ? lstUser.OrderBy(u => u.Name) : lstUser.OrderByDescending(u => u.Name);
                    break;

                default:
                    lstUser = lstUser.OrderBy(u => u.UserId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstUser.Count();

            //pagination code start******************************************************
            lstUser = lstUser.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<UserViewModel> lstModel = new List<UserViewModel>();

            foreach (User obj in lstUser)
            {
                UserViewModel model = new UserViewModel();
                model.UserId = obj.UserId;
                model.Name = obj.Name;
                model.Email = obj.Email;
                model.ContactNo = obj.ContactNo;
                model.Image = obj.Image;
                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.UserList = lstModel;
            return gridModel;
        }

        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                User c = db.Users.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }


        public bool Delete(int id)
        {
            try
            {
                User obj = db.Users.Find(id);

                if (obj != null)
                {
                    db.Users.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public UpdateUserViewModel GetUserById(int uid)
        {
            try
            {
                UpdateUserViewModel model = new UpdateUserViewModel();
                User obj = db.Users.Find(uid);
                if (obj != null)
                {
                    model.UserId = obj.UserId;
                    model.Image = obj.Image;
                    model.Name = obj.Name;
                    model.Address = obj.Address;
                    model.ContactNo = obj.ContactNo;
                    model.Gender = obj.Gender;
                    model.Email = obj.Email;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        public string GetUserAddr(int uid)
        {
            string address = db.Users.Where(u => u.UserId == uid).Select(u => u.Address).FirstOrDefault();
            return address;
        }


        public int GetUserCnt()
        {
            int totalUrs = db.Users.Where(u => u.UserTypeId != 1).Count();
            return totalUrs;
        }


        public bool UpdateUserProfile(UpdateUserViewModel model)
        {
            try
            {
                User obj = db.Users.Find(model.UserId);
                if (obj != null)
                {
                    obj.UserId = model.UserId;
                    obj.Name = model.Name;
                    obj.ContactNo = model.ContactNo;
                    obj.Gender = model.Gender;
                    obj.Address = model.Address;
                    db.Entry<User>(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
        }


        public bool RegisterUser(RegisterViewModel model)
        {
            try
            {
                bool flag = false;
                flag = IsDuplicate(model.Email);
                if (flag == true)
                    return false;

                User uobj = new User();
                uobj.UserId = NewId();
                uobj.Name = model.Name;
                uobj.Email = model.Email;
                uobj.ContactNo = model.ContactNo;
                uobj.Password = model.Password;
                uobj.CreatedDt = DateTime.Now;
                uobj.UserTypeId = 2;
                uobj.RoleId = 2;
                uobj.Status = true;

                db.Users.Add(uobj);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
        }
    }
}
