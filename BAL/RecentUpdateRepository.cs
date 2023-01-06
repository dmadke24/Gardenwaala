using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data.Entity.Validation;
using GardenViewModel;

namespace BAL
{
    public interface IRecentUpdateRepository
    {
        bool Add(RecentUpdateViewModel model);
        bool Update(RecentUpdateViewModel model);
        bool Delete(int id);
        RecentUpdateViewModel GetById(int id);
        RecentUpdateGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);

    }


    public class RecentUpdateRepository : IRecentUpdateRepository
    {
        MyDBContext db = new MyDBContext();

        private int NewId()
        {
            string oldid = Convert.ToString(db.RecentUpdates.Select(ru => ru.UpdateId)
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

        public bool Add(RecentUpdateViewModel model)
        {
            try
            {
                RecentUpdate obj = new RecentUpdate();
                obj.UpdateId = NewId(); //Id generated manually
                obj.Title = model.Title;
                obj.UpdateDt = DateTime.Now;
                obj.Description = model.Description;
                obj.Image = model.Image;
                obj.Image1 = model.Image1;
                obj.Image2 = model.Image2;
                obj.Link = model.Link;
                obj.ScheduleDt = Convert.ToDateTime(model.ScheduleDt);
                obj.Status = model.Status;

                //Add to DB
                db.RecentUpdates.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {

                return false;
            }

        }

        public bool Update(RecentUpdateViewModel model)
        {
            try
            {
                RecentUpdate obj = new RecentUpdate();
                obj.UpdateId = model.UpdateId;
                obj.Title = model.Title;
                obj.UpdateDt = DateTime.Now;
                obj.Description = model.Description;
                obj.Image = model.Image;
                obj.Image1 = model.Image1;
                obj.Image2 = model.Image2;
                obj.Link = model.Link;

                obj.ScheduleDt = Convert.ToDateTime(model.ScheduleDt);
                obj.Status = model.Status;

                db.Entry<RecentUpdate>(obj).State = EntityState.Modified;

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
                RecentUpdate obj = db.RecentUpdates.Find(id);

                if (obj != null)
                {
                    db.RecentUpdates.Remove(obj);
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

        public RecentUpdateViewModel GetById(int id)
        {
            try
            {
                RecentUpdateViewModel model = new RecentUpdateViewModel();
                RecentUpdate obj = db.RecentUpdates.Find(id);
                if (obj != null)
                {
                    model.UpdateId = obj.UpdateId;
                    model.Title = obj.Title;
                    model.UpdateDt = obj.UpdateDt.ToString("dd-MMM-yyyy");
                    model.Description = obj.Description;
                    model.Image = obj.Image;
                    model.Image1 = obj.Image1;
                    model.Image2 = obj.Image2;

                    model.Link = obj.Link;

                    model.ScheduleDt = obj.ScheduleDt.ToString("yyyy-MM-dd hh:mm:ss T");

                    model.Status = obj.Status;
                    model.OldFile = obj.Image;
                    model.OldFile1 = obj.Image1;
                    model.OldFile2 = obj.Image2;

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
        public RecentUpdateGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF RecentUpdateGridViewModel'
            RecentUpdateGridViewModel gridModel = new RecentUpdateGridViewModel();

            IQueryable<RecentUpdate> lstRecentUpdate = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstRecentUpdate = db.RecentUpdates;
            }
            else
            {
                lstRecentUpdate = db.RecentUpdates.Where(ru => ru.Title.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "title":
                    lstRecentUpdate = sortDir == "asc" ? lstRecentUpdate.OrderBy(ru => ru.Title) : lstRecentUpdate.OrderByDescending(ru => ru.Title);
                    break;

                case "updatedt":
                    lstRecentUpdate = sortDir == "asc" ? lstRecentUpdate.OrderBy(ru => ru.UpdateDt) : lstRecentUpdate.OrderByDescending(ru => ru.UpdateDt);
                    break;

                case "scheduledt":
                    lstRecentUpdate = sortDir == "asc" ? lstRecentUpdate.OrderBy(ru => ru.ScheduleDt) : lstRecentUpdate.OrderByDescending(ru => ru.ScheduleDt);
                    break;

                default:
                    lstRecentUpdate = lstRecentUpdate.OrderBy(ru => ru.UpdateId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstRecentUpdate.Count();

            //pagination code start******************************************************
            lstRecentUpdate = lstRecentUpdate.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstRecentUpdate to CategoryList in gridModel i.e. RecentUpdateViewModel
            List<RecentUpdateViewModel> lstModel = new List<RecentUpdateViewModel>();

            foreach (RecentUpdate obj in lstRecentUpdate)
            {
                RecentUpdateViewModel model = new RecentUpdateViewModel();
                model.UpdateId = obj.UpdateId;
                model.Title = obj.Title;
                model.UpdateDt = obj.UpdateDt.ToString("dd-MMM-yyyy");
                model.Description = obj.Description;
                model.Image = obj.Image;
                model.Image1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.Link = obj.Link;

                model.ScheduleDt = obj.ScheduleDt.ToString("dd-MMM-yyyy hh:mm tt");

                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.RecentUpdateList = lstModel;
            return gridModel;
        }

    }
}
