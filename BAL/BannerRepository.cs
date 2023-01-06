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
    public interface IBannerRepository
    {
        //bool Add(BannerViewModel model);
        bool Add(BannerViewModel model);
        bool Update(BannerViewModel model);
        bool Delete(int id);
        BannerViewModel GetById(int id);//here this id is used for edit purpose
        BannerGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<BannerViewModel> GetHomeBanner();
    }
    public class BannerRepository : IBannerRepository
    {
        MyDBContext db = new MyDBContext();

        //Generating new Id Menually
        private int NewId()
        {
            string oldid = Convert.ToString(db.Banners.Select(b => b.BannerId)
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

        public bool Add(BannerViewModel model)
        {
            try
            {
                Banner obj = new Banner();
                obj.BannerId = NewId(); //Id generated manually
                obj.Image = model.Image;
                obj.Title = model.Title;
                obj.Status = model.Status;

                //Add to DB
                db.Banners.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(BannerViewModel model)
        {

            try
            {
                Banner obj = new Banner();
                obj.BannerId = model.BannerId;
                obj.Image = model.Image;
                obj.Title = model.Title;
                obj.Status = model.Status;

                db.Entry<Banner>(obj).State = EntityState.Modified;
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
                Banner obj = db.Banners.Find(id);

                if (obj != null)
                {
                    db.Banners.Remove(obj);
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

        public BannerViewModel GetById(int id)
        {
            try
            {
                BannerViewModel model = new BannerViewModel();
                Banner obj = db.Banners.Find(id);
                if (obj != null)
                {
                    model.BannerId = obj.BannerId;
                    model.Image = obj.Image;
                    model.Title = obj.Title;
                    model.Status = obj.Status;
                    model.OldFile = obj.Image;

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

        public BannerGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            BannerGridViewModel gridModel = new BannerGridViewModel();

            IQueryable<Banner> lstBanner = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstBanner = db.Banners;
            }
            else
            {
                lstBanner = db.Banners.Where(b => b.Title.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "title":
                    lstBanner = sortDir == "asc" ? lstBanner.OrderBy(b => b.Title) : lstBanner.OrderByDescending(b => b.Title);
                    break;

                default:
                    lstBanner = lstBanner.OrderBy(b => b.BannerId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstBanner.Count();

            //pagination code start******************************************************
            lstBanner = lstBanner.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<BannerViewModel> lstModel = new List<BannerViewModel>();

            foreach (Banner obj in lstBanner)
            {
                BannerViewModel model = new BannerViewModel();
                model.BannerId = obj.BannerId;
                model.Image = obj.Image;
                model.Title = obj.Title;
                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.BannerList = lstModel;
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

        public IEnumerable<BannerViewModel> GetHomeBanner()
        {

            IQueryable<Banner> lstBann = null;

            lstBann = db.Banners.Where(b => b.Status == true);
            List<BannerViewModel> lstModel = new List<BannerViewModel>();
            foreach (Banner obj in lstBann)
            {
                BannerViewModel model = new BannerViewModel();
                model.BannerId = obj.BannerId;
                model.Image = obj.Image;
                model.Title = obj.Title;
                lstModel.Add(model);
            }

            return lstModel;
        }

    }
}
