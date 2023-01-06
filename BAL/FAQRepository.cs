using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;

namespace BAL
{
    public interface IFAQRepository
    {
        bool Add(FAQViewModel model);
        bool Update(FAQViewModel model);
        bool Delete(int id);
        FAQViewModel GetById(int id);
        FAQGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<FAQViewModel> GetHomeFAQ();
    }

    public class FAQRepository : IFAQRepository
    {

        MyDBContext db = new MyDBContext();
        private int NewId()
        {
            string oldid = Convert.ToString(db.FAQs.Select(f => f.FaqId)
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
        public bool Add(FAQViewModel model)
        {
            try
            {
                FAQ obj = new FAQ();
                obj.FaqId = NewId();
                obj.Question = model.Question;
                obj.Answer = model.Answer;
                obj.Status = model.Status;
                db.FAQs.Add(obj);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(FAQViewModel model)
        {
            try
            {
                FAQ obj = new FAQ();
                obj.FaqId = model.FaqId;
                obj.Question = model.Question;
                obj.Answer = model.Answer;
                obj.Status = model.Status;
                db.Entry<FAQ>(obj).State = System.Data.Entity.EntityState.Modified;
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
                FAQ obj = db.FAQs.Find(id);
                if (obj != null)
                {
                    db.FAQs.Remove(obj);
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

        public FAQViewModel GetById(int id)
        {
            try
            {
                FAQViewModel model = new FAQViewModel();

                FAQ obj = db.FAQs.Find(id);
                if (obj != null)
                {
                    model.FaqId = obj.FaqId;
                    model.Question = obj.Question;
                    model.Answer = obj.Answer;
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

        public FAQGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            FAQGridViewModel gridModel = new FAQGridViewModel();

            IQueryable<FAQ> lstFAQ = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstFAQ = db.FAQs;
            }
            else
            {
               lstFAQ = db.FAQs.Where(f => f.Question.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "question":
                    lstFAQ = sortDir == "asc" ? lstFAQ.OrderBy(f => f.Question) : lstFAQ.OrderByDescending(f => f.Question);
                    break;

                default:
                    lstFAQ = lstFAQ.OrderBy(f => f.FaqId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstFAQ.Count();

            //pagination code start******************************************************
            lstFAQ = lstFAQ.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<FAQViewModel> lstModel = new List<FAQViewModel>();

            foreach (FAQ obj in lstFAQ)
            {
                FAQViewModel model = new FAQViewModel();

                model.FaqId = obj.FaqId;
                model.Question = obj.Question;
                model.Answer = obj.Answer;
                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.FAQList = lstModel;
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


        public IEnumerable<FAQViewModel> GetHomeFAQ()
        {
            IQueryable<FAQ> lstFaq = null;

            lstFaq = db.FAQs.Where(b => b.Status == true);
            List<FAQViewModel> lstModel = new List<FAQViewModel>();
            foreach (FAQ obj in lstFaq)
            {
                FAQViewModel model = new FAQViewModel();
                model.Question = obj.Question;
                model.Answer = obj.Answer;
                lstModel.Add(model);
            }

            return lstModel;
        }
    }
}
