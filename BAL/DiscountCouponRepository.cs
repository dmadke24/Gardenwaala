using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;

namespace BAL
{
    public interface IDiscountCouponRepository
    {
        bool Add(DiscountCouponViewModel model);
        bool Update(DiscountCouponViewModel model);
        bool Delete(int id);
        DiscountCouponViewModel GetById(int id);
        DiscountCouponGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        IEnumerable<DropDownViewModel> GetAll();
        IEnumerable<StringDropDownViewModel> GetIDPercent();
        float GetCouponAmount(int cid);
        IEnumerable<DiscountCouponViewModel> HomeCouponImage();
        float ValidateCoupon(string CouponCode);
    }

    public class DiscountCouponRepository : IDiscountCouponRepository
    {
        MyDBContext db = new MyDBContext();
        private int NewId()
        {
            string oldid = Convert.ToString(db.DiscountCoupons.Select(dc => dc.CouponId)
                                                            .DefaultIfEmpty().Max());
            string myid, newid;
            int year = DateTime.Now.Year;
            if (oldid == "0")
            {

                myid = year + "00001";
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
                    newid = old_year + old_no.ToString("00000");
                }
                else
                    newid = year + "00001";

                return (Convert.ToInt32(newid));
            }

        }

        public bool Add(DiscountCouponViewModel model)
        {
            try
            {
                DiscountCoupon obj = new DiscountCoupon();
                obj.CouponId = NewId();
                obj.CategoryId = model.CategoryId;
                obj.Code = model.Code;
                obj.Title = model.Title;
                obj.Image = model.Image;
                string[] dtParts = model.ValidFrom.Split('-');
                obj.ValidFrom = new DateTime(Convert.ToInt32(dtParts[2]), Convert.ToInt32(dtParts[1]), Convert.ToInt32(dtParts[0]));
                dtParts = model.ValidTo.Split('-');
                obj.ValidTo = new DateTime(Convert.ToInt32(dtParts[2]), Convert.ToInt32(dtParts[1]), Convert.ToInt32(dtParts[0]));
                obj.DiscountPercent = model.DiscountPercent;
                db.DiscountCoupons.Add(obj);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DiscountCouponViewModel model)
        {
            try
            {
                DiscountCoupon obj = new DiscountCoupon();
                obj.CouponId = model.CouponId;
                obj.CategoryId = model.CategoryId;
                obj.Code = model.Code;
                obj.Title = model.Title;
                obj.Image = model.Image;
                string[] dtParts = model.ValidFrom.Split('-');
                obj.ValidFrom = new DateTime(Convert.ToInt32(dtParts[2]), Convert.ToInt32(dtParts[1]), Convert.ToInt32(dtParts[0]));
                dtParts = model.ValidTo.Split('-');
                obj.ValidTo = new DateTime(Convert.ToInt32(dtParts[2]), Convert.ToInt32(dtParts[1]), Convert.ToInt32(dtParts[0]));
                obj.DiscountPercent = model.DiscountPercent;
                db.Entry<DiscountCoupon>(obj).State = System.Data.Entity.EntityState.Modified;
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
                DiscountCoupon obj = db.DiscountCoupons.Find(id);
                if (obj != null)
                {
                    db.DiscountCoupons.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public DiscountCouponViewModel GetById(int id)
        {
            DiscountCouponViewModel model = null;
            DiscountCoupon obj = db.DiscountCoupons.Find(id);
            if (obj != null)
            {
                model = new DiscountCouponViewModel();
                model.CouponId = obj.CouponId;
                model.Code = obj.Code;
                model.Title = obj.Title;
                model.Image = obj.Image;
                model.OldFile = obj.Image;
                model.CategoryId = obj.CategoryId.Value;
                model.ValidFrom = obj.ValidFrom.ToString("dd-MM-yyyy");
                model.ValidTo = obj.ValidTo.ToString("dd-MM-yyyy");
                model.DiscountPercent = obj.DiscountPercent;
            }
            return model;
        }

        public DiscountCouponGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            DiscountCouponGridViewModel gridModel = new DiscountCouponGridViewModel();

            IQueryable<DiscountCoupon> lstCoupon = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstCoupon = db.DiscountCoupons;
            }
            else
            {
                lstCoupon = db.DiscountCoupons.Where(dc => (dc.Title.ToLower().Contains(searchStr) || dc.Code.ToLower().Contains(searchStr)));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "title":
                    lstCoupon = sortDir == "asc" ? lstCoupon.OrderBy(dc => dc.Title) : lstCoupon.OrderByDescending(dc => dc.Title);
                    break;

                default:
                    lstCoupon = lstCoupon.OrderBy(dc => dc.CouponId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstCoupon.Count();

            //pagination code start******************************************************
            lstCoupon = lstCoupon.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<DiscountCouponViewModel> lstModel = new List<DiscountCouponViewModel>();

            foreach (DiscountCoupon obj in lstCoupon.ToList())
            {
                DiscountCouponViewModel model = new DiscountCouponViewModel();

                model.CouponId = obj.CouponId;
                model.Code = obj.Code;
                model.Title = obj.Title;
                model.CategoryId = obj.CategoryId.Value;
                model.ValidFrom = obj.ValidFrom.ToString("dd-MMM-yyyy");
                model.ValidTo = obj.ValidTo.ToString("dd-MMM-yyyy");
                model.DiscountPercent = obj.DiscountPercent;

                lstModel.Add(model);
            }

            gridModel.CouponList = lstModel;
            return gridModel;
        }

        public IEnumerable<StringDropDownViewModel> GetIDPercent()
        {
            IEnumerable<StringDropDownViewModel> lstModel = db.Database.SqlQuery<StringDropDownViewModel>("Select CAST(CouponId As VARCHAR(10)) + ',' + CAST(DiscountPercent As VARCHAR(10)) As ID, Title As Text From DiscountCoupons Where DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) between ValidFrom and ValidTo");
            return lstModel;
        }

        public IEnumerable<DropDownViewModel> GetAll()
        {
            IEnumerable<DropDownViewModel> lstModel = db.Database.SqlQuery<DropDownViewModel>("Select CouponId  As ID, Title As Text From DiscountCoupons Where DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) between ValidFrom and ValidTo");
            return lstModel;
        }
        public float GetCouponAmount(int cid)
        {
            float DiscPercent = db.Database.SqlQuery<float>("Select DiscountPercent From DiscountCoupons Where CouponId=" + cid).FirstOrDefault();
            return DiscPercent;
        }


        public IEnumerable<DiscountCouponViewModel> HomeCouponImage()
        {
            IEnumerable<DiscountCouponViewModel> lstModel = db.Database.SqlQuery<DiscountCouponViewModel>("Select DiscountPercent,Code,Image,Title,CouponId From DiscountCoupons Where ValidTo >= CAST(getdate() AS DATE)");
            return lstModel;
        }

        public float ValidateCoupon(string CouponCode)
        {
            float DiscPercent = db.Database.SqlQuery<float>("Select DiscountPercent From DiscountCoupons Where Code='" + CouponCode + "' and ValidTo >= CAST(getdate() AS DATE)").FirstOrDefault();
            return DiscPercent;
        }
    }
}
