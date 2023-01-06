using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using GardenViewModel;

namespace BAL
{

    public class CityRepository : ICityRepository
    {
        MyDBContext db = new MyDBContext();
        public bool Add(CityViewModel model)
        {
            try
            {
                City obj = new City();
                obj.CityName = model.CityName;
                obj.StateId = model.StateId;
                obj.Status = model.Status;
                db.Cities.Add(obj);

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(CityViewModel model)
        {
            try
            {
                City obj = new City();
                obj.CityName = model.CityName;
                obj.StateId = model.StateId;
                obj.CityId = model.CityId;
                obj.Status = model.Status;


                db.Entry<City>(obj).State = EntityState.Modified;
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
                City obj = db.Cities.Find(id);
                if (obj != null)
                {
                    db.Cities.Remove(obj);
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

        public CityViewModel GetById(int id)
        {
            try
            {
                CityViewModel model = new CityViewModel();

                City obj = db.Cities.Find(id);
                if (obj != null)
                {
                    model.CityId = obj.CityId;
                    model.StateId = obj.StateId;
                    model.CityName = obj.CityName;
                    model.Status = obj.Status;
                    return model;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public CityGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CityGridViewModel'
            CityGridViewModel gridModel = new CityGridViewModel();

            IQueryable<City> lstCity = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstCity = db.Cities;
            }
            else
            {
                lstCity = db.Cities.Where(c => c.CityName.ToLower().Contains(searchStr) || c.State.SName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "sname":
                    lstCity = sortDir == "asc" ? lstCity.OrderBy(c => c.State.SName) : lstCity.OrderByDescending(s => s.State.SName);
                    break;

                case "cityname":
                    lstCity = sortDir == "asc" ? lstCity.OrderBy(c => c.CityName) : lstCity.OrderByDescending(c => c.CityName);
                    break;

                default:
                    lstCity = lstCity.OrderBy(c => c.CityId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstCity.Count();

            //pagination code start******************************************************
            lstCity = lstCity.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCity to CityList in gridModel i.e. CityGridViewModel
            List<CityViewModel> lstModel = new List<CityViewModel>();

            foreach (City obj in lstCity.ToList<City>())
            {
                CityViewModel model = new CityViewModel();

                model.CityId = obj.CityId;
                model.StateId = obj.StateId;
                model.CityName = obj.CityName;
                model.SName = obj.State.SName;
                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.CityList = lstModel;
            return gridModel;
        }
        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                City c = db.Cities.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }


        public IEnumerable<DropDownViewModel> GetCities()
        {
            IEnumerable<DropDownViewModel> lstCity =
                db.Database.SqlQuery<DropDownViewModel>("Select CityId as ID, CityName as Text From Cities where Status=1");
            return lstCity;
        }
    }
}
