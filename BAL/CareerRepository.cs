using GardenViewModel;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BAL
{
    public class CareerRepository : ICareerRepository
    {
        MyDBContext db = new MyDBContext();

        //Generating new Id Menually
        private int NewId()
        {
            string oldid = Convert.ToString(db.Careers.Select(c => c.JobTitleId)
                                                            .DefaultIfEmpty().Max());
            string myid, newid;
            int year = DateTime.Now.Year;
            if (oldid == "0")
            {

                myid = year + "0001";
                return Convert.ToInt32(myid);
            }
            else
            {

                //Split the id into two parts year and no
                string old_year = oldid.Substring(0, 4);
                int old_no = Convert.ToInt32(oldid.Substring(4));

                if (year == Convert.ToInt32(old_year))
                {
                    old_no += 1;
                    newid = old_year + old_no.ToString("0000");
                }
                else
                    newid = year + "0001";

                return (Convert.ToInt32(newid));
            }

        }

        public bool Add(CareerViewModel model)
        {
            try
            {
                Career obj = new Career();
                obj.JobTitleId = NewId();     //Id generated menually
                obj.Position = model.Position;
                obj.Experience = model.Experience;
                obj.Requirements = model.Requirements;
                obj.Description = model.Description;
                obj.Location = model.Location;
                obj.City = model.City;
                obj.CreatedDt = DateTime.Now;

                obj.Qualification = model.Qualification;
                obj.Department = model.Department;
                obj.Package = model.Package;
                obj.KeySkills = model.keyskills;
                obj.Openings = model.Openings;

                obj.Status = model.Status;

                //Add to DB
                db.Careers.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }


        public bool Update(CareerViewModel model)
        {
            try
            {
                Career obj = new Career();
                obj.JobTitleId = model.JobTitleId;
                obj.Position = model.Position;
                obj.Experience = model.Experience;
                obj.Requirements = model.Requirements;
                obj.Description = model.Description;
                obj.Location = model.Location;
                obj.City = model.City;
                obj.CreatedDt = model.CreatedDt;

                obj.Qualification = model.Qualification;
                obj.Department = model.Department;
                obj.Package = model.Package;
                obj.KeySkills = model.keyskills;
                obj.Openings = model.Openings;

                obj.Status = model.Status;

                //Update DB
                db.Entry<Career>(obj).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }

        }

        public bool Delete(int id)
        {
            try
            {
                Career obj = db.Careers.Find(id);
                if (obj != null)
                {
                    db.Careers.Remove(obj);
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



        public CareerViewModel GetById(int id)
        {
            try
            {
                CareerViewModel model = new CareerViewModel();
                Career obj = db.Careers.Find(id);

                if (obj != null)
                {
                    model.JobTitleId = obj.JobTitleId;
                    model.Position = obj.Position;
                    model.Experience = obj.Experience;
                    model.Requirements = obj.Requirements;
                    model.Description = obj.Description;
                    model.Location = obj.Location;
                    model.City = obj.City;
                    model.CreatedDt = obj.CreatedDt;

                    model.Qualification = obj.Qualification;
                    model.Department = obj.Department;
                    model.Package = obj.Package;
                    model.keyskills = obj.KeySkills;
                    model.Openings = obj.Openings;

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

        public CareerGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {

            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            CareerGridViewModel gridModel = new CareerGridViewModel();

            IQueryable<Career> lstCareer = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstCareer = db.Careers;
            }
            else
            {
                lstCareer = db.Careers.Where(c => c.Position.ToLower().Contains(searchStr));

            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "position":
                    lstCareer = sortDir == "asc" ? lstCareer.OrderBy(c => c.Position) : lstCareer.OrderByDescending(c => c.Position);
                    break;

                case "city":
                    lstCareer = sortDir == "asc" ? lstCareer.OrderBy(c => c.City) : lstCareer.OrderByDescending(c => c.City);
                    break;

                case "location":
                    lstCareer = sortDir == "asc" ? lstCareer.OrderBy(c => c.Location) : lstCareer.OrderByDescending(c => c.Location);
                    break;

                case "createdDt":
                    lstCareer = sortDir == "asc" ? lstCareer.OrderBy(c => c.CreatedDt) : lstCareer.OrderByDescending(c => c.CreatedDt);
                    break;

                case "department":
                    lstCareer = sortDir == "asc" ? lstCareer.OrderBy(c => c.Department) : lstCareer.OrderByDescending(c => c.Department);
                    break;



                default:
                    lstCareer = lstCareer.OrderBy(c => c.JobTitleId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstCareer.Count();

            //pagination code start******************************************************
            lstCareer = lstCareer.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<CareerViewModel> lstModel = new List<CareerViewModel>();

            foreach (Career obj in lstCareer.ToList<Career>())
            {
                CareerViewModel model = new CareerViewModel();

                model.JobTitleId = obj.JobTitleId;
                model.Position = obj.Position;
                model.Experience = obj.Experience;
                model.Requirements = obj.Requirements;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description.Substring(0, obj.Description.Length <= 100 ? obj.Description.Length : 100) + " ...";
                model.Location = obj.Location;
                model.City = obj.City;
                model.CreatedDt = obj.CreatedDt;

                model.Qualification = obj.Qualification;
                model.Department = obj.Department;
                model.Package = obj.Package;
                model.keyskills = obj.KeySkills;
                model.Openings = obj.Openings;

                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.CareerList = lstModel;
            return gridModel;
        }


        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                Banner c = db.Banners.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }

        public IEnumerable<CareerViewModel> GetHomeJob()
        {
            IEnumerable<CareerViewModel> lstmodel = db.Database.SqlQuery<CareerViewModel>("select * from Careers where Status=1 order by JobTitleId Desc");
            return lstmodel;
        }
    }
}
