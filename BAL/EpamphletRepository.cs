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
    public interface IEpamphletRepository
    {
        bool Add(EpamphletViewModel model);
        EpamphletGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
    }


    public class EpamphletRepository : IEpamphletRepository
    {
        MyDBContext db = new MyDBContext();

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

        public bool Add(EpamphletViewModel model)
        {
            try
            {
                Epamphlet obj = new Epamphlet();
                obj.EpamphletId = NewId();
                obj.Subject = model.Subject;
                obj.Message = model.Message;
                obj.Link = model.Link;
                obj.Image = model.Image;
                obj.CreateDate = DateTime.Now;

                //Add to DB
                db.Epamphlets.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public EpamphletGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            EpamphletGridViewModel gridModel = new EpamphletGridViewModel();

            IQueryable<Epamphlet> lstEpamphlet = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstEpamphlet = db.Epamphlets;
            }
            else
            {
                lstEpamphlet = db.Epamphlets.Where(e => e.Subject.ToLower().Contains(searchStr));

            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "createdate":
                    lstEpamphlet = sortDir == "asc" ? lstEpamphlet.OrderBy(e => e.CreateDate) : lstEpamphlet.OrderByDescending(e => e.CreateDate);
                    break;

                default:
                    lstEpamphlet = lstEpamphlet.OrderBy(e => e.EpamphletId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstEpamphlet.Count();

            //pagination code start******************************************************
            lstEpamphlet = lstEpamphlet.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<EpamphletViewModel> lstModel = new List<EpamphletViewModel>();

            foreach (Epamphlet obj in lstEpamphlet)
            {
                EpamphletViewModel model = new EpamphletViewModel();

                model.EpamphletId = obj.EpamphletId;
                model.Subject = obj.Subject;
                model.Message = obj.Message;
                model.Link = obj.Link;
                model.Image = obj.Image;
                model.DisplayCDate = obj.CreateDate.ToString("dd-MMM-yyyy");
                lstModel.Add(model);
            }

            gridModel.EpamphletList = lstModel;
            return gridModel;
        }
    }
}
