using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenViewModel;
using DAL;
using System.Data.Entity;

namespace BAL
{
    public class CountryRepository : ICountryRepository
    {
        MyDBContext db = new MyDBContext();
        public bool Add(CountryViewModel model)
        {
            try
            {
                Country obj = new Country   ();
                obj.CName = model.CName;
                obj.CountryId = model.CountryId;
                db.Countries.Add(obj);

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(CountryViewModel model)
        {
            try
            {
                Country obj = new Country();
                obj.CName = model.CName;
                obj.CountryId = model.CountryId;

                db.Entry<Country>(obj).State = EntityState.Modified;
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
                Country obj = db.Countries.Find(id);
                if (obj != null)
                {
                    db.Countries.Remove(obj);
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

        public CountryViewModel GetById(int id)
        {
            try
            {
                CountryViewModel model = new CountryViewModel();

                Country obj = db.Countries.Find(id);
                if (obj != null)
                {
                    model.CountryId = obj.CountryId;
                    model.CName = obj.CName;
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

        public CountryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CountryGridViewModel'
            CountryGridViewModel gridModel = new CountryGridViewModel();

            IQueryable<Country> lstCountry = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstCountry = db.Countries;
            }
            else
            {
                lstCountry = db.Countries.Where(c => c.CName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "cname":
                    lstCountry = sortDir == "asc" ? lstCountry.OrderBy(c => c.CName) : lstCountry.OrderByDescending(c => c.CName);
                    break;

                default:
                    lstCountry = lstCountry.OrderBy(c => c.CountryId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstCountry.Count();

            //pagination code start******************************************************
            lstCountry = lstCountry.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCountry to CountryList in gridModel i.e. CountryGridViewModel
            List<CountryViewModel> lstModel = new List<CountryViewModel>();

            foreach (Country obj in lstCountry)
            {
                CountryViewModel model = new CountryViewModel();

                model.CountryId = obj.CountryId;
                model.CName = obj.CName;

                lstModel.Add(model);
            }

            gridModel.CountryList = lstModel;
            return gridModel;
        }


        public IEnumerable<DropDownViewModel> GetCountries()
        {
            //raw SQL query
            IEnumerable<DropDownViewModel> lstCountry = db.Database.SqlQuery<DropDownViewModel>("Select CountryId as ID, CName as Text From Countries");
            return lstCountry;
        }
    }
}
