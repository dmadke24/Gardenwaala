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
    public class AreaRepository : IAreaRepository
    {

        MyDBContext db = new MyDBContext();
        public bool Add(AreaViewModel model)
        {
            try
            {
                Area obj = new Area();
                obj.AreaId = model.AreaId;
                obj.AName = model.AName;
                obj.CityId = 1;
                obj.Pincode = model.Pincode;
                obj.Status = model.Status;
                db.Areas.Add(obj);

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(AreaViewModel model)
        {
            try
            {
                Area obj = new Area();
                obj.AName = model.AName;
                obj.AreaId = model.AreaId;
                obj.CityId = 1;
                obj.Status = model.Status;
                obj.Pincode = model.Pincode;

                db.Entry<Area>(obj).State = EntityState.Modified;
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
                Area obj = db.Areas.Find(id);
                if (obj != null)
                {
                    db.Areas.Remove(obj);
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

        public AreaViewModel GetById(int id)
        {
            try
            {
                AreaViewModel model = new AreaViewModel();

                Area obj = db.Areas.Find(id);
                if (obj != null)
                {
                    model.CityId = obj.CityId;
                    model.AreaId = obj.AreaId;
                    model.AName = obj.AName;
                    model.Pincode = obj.Pincode;
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

        public AreaGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF AreaGridViewModel'
            AreaGridViewModel gridModel = new AreaGridViewModel();

            IQueryable<Area> lstArea = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstArea = db.Areas;
            }
            else
            {
                lstArea = db.Areas.Where(a => a.AName.ToLower().Contains(searchStr) || a.City.CityName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "cityname":
                    lstArea = sortDir == "asc" ? lstArea.OrderBy(a => a.City.CityName) : lstArea.OrderByDescending(a => a.City.CityName);
                    break;

                case "aname":
                    lstArea = sortDir == "asc" ? lstArea.OrderBy(a => a.AName) : lstArea.OrderByDescending(a => a.AName);
                    break;

                default:
                    lstArea = lstArea.OrderBy(a => a.AreaId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstArea.Count();

            //pagination code start******************************************************
            lstArea = lstArea.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCity to CityList in gridModel i.e. CityGridViewModel
            List<AreaViewModel> lstModel = new List<AreaViewModel>();

            foreach (Area obj in lstArea.ToList<Area>())
            {
                AreaViewModel model = new AreaViewModel();

                model.CityId = obj.CityId;
                model.AreaId = obj.AreaId;
                model.CityName = obj.City.CityName;
                model.AName = obj.AName;
                model.Status = obj.Status;
                model.Pincode = obj.Pincode;
                lstModel.Add(model);
            }

            gridModel.AreaList = lstModel;
            return gridModel;
        }
        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                Area c = db.Areas.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }

        //this method will return list of Areas by 'city'
        public IEnumerable<AreaViewModel> GetAreas(int cid)
        {
            IQueryable<Area> lstArea = db.Areas.Where(a => a.CityId == cid && a.Status == true);
            List<AreaViewModel> lstModel = new List<AreaViewModel>();
            foreach (Area obj in lstArea)
            {
                AreaViewModel model = new AreaViewModel();

                model.AreaId = obj.AreaId;
                model.AName = obj.AName;
                model.Pincode = obj.Pincode;
                lstModel.Add(model);
            }
            return lstModel;

        }

        public bool DeliveryArea(string pincode)
        {
            bool flag = false;
            flag = db.Areas.Where(a => a.Pincode == pincode).Count() > 0;
            return flag;
        }
    }
}
