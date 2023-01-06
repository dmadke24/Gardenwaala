using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using DAL;
using System.Data.Entity;
using GardenViewModel;

namespace BAL
{
    public class TestimonialRepository : ITestimonialRepository
    {
        MyDBContext db = new MyDBContext();

        //Generating new Id Menually
        private int NewId()
        {
            string oldid = Convert.ToString(db.Testimonials.Select(t => t.TestimonialId)
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

        public bool Add(TestimonialViewModel model)
        {
            try
            {
                Testimonial obj = new Testimonial();

                obj.TestimonialId = NewId(); //Id generated menually
                obj.CName = model.CName;
                obj.Image = model.Image;
                obj.Profession = model.Profession;
                obj.Organization = model.Organization;
                obj.Description = model.Description;
                obj.Status = model.Status;

                //Add to DB
                db.Testimonials.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool Update(TestimonialViewModel model)
        {
            try
            {
                Testimonial obj = new Testimonial();

                obj.TestimonialId = model.TestimonialId;
                obj.CName = model.CName;
                obj.Image = model.Image;
                obj.Profession = model.Profession;
                obj.Organization = model.Organization;
                obj.Description = model.Description;
                obj.Status = model.Status;

                //Update DB
                db.Entry<Testimonial>(obj).State = EntityState.Modified;
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
                Testimonial obj = db.Testimonials.Find(id);

                if (obj != null)
                {
                    db.Testimonials.Remove(obj);
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

        public TestimonialViewModel GetById(int id)
        {
            try
            {
                TestimonialViewModel model = new TestimonialViewModel();
                Testimonial obj = db.Testimonials.Find(id);

                if (obj != null)
                {
                    model.TestimonialId = obj.TestimonialId;
                    model.CName = obj.CName;
                    model.Image = obj.Image;
                    model.Profession = obj.Profession;
                    model.Organization = obj.Organization;
                    model.Description = obj.Description;
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

        public TestimonialGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            TestimonialGridViewModel gridModel = new TestimonialGridViewModel();

            IQueryable<Testimonial> lstTestimonial = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstTestimonial = db.Testimonials;
            }
            else
            {
                lstTestimonial = db.Testimonials.Where(t => t.CName.ToLower().Contains(searchStr));

            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "cname":
                    lstTestimonial = sortDir == "asc" ? lstTestimonial.OrderBy(t => t.CName) : lstTestimonial.OrderByDescending(t => t.CName);
                    break;

                default:
                    lstTestimonial = lstTestimonial.OrderBy(t => t.TestimonialId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstTestimonial.Count();

            //pagination code start******************************************************
            lstTestimonial = lstTestimonial.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<TestimonialViewModel> lstModel = new List<TestimonialViewModel>();

            foreach (Testimonial obj in lstTestimonial)
            {
                TestimonialViewModel model = new TestimonialViewModel();

                model.TestimonialId = obj.TestimonialId;
                model.CName = obj.CName;
                model.Image = obj.Image;
                model.Profession = obj.Profession;
                model.Organization = obj.Organization;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description.Substring(0, obj.Description.Length <= 100 ? obj.Description.Length : 100) + " ...";
                model.Status = obj.Status;

                lstModel.Add(model);
            }

            gridModel.TestimonialList = lstModel;
            return gridModel;
        }

        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                Testimonial c = db.Testimonials.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }


        public string GetFileName(int id)
        {
            string sql = "select Image from Testimonials where TestimonialId=" + id.ToString();
            string Image = db.Database.SqlQuery<string>(sql).FirstOrDefault();
            return Image;

        }

        public IEnumerable<TestimonialViewModel> GetHomeTestimonial()
        {
            IQueryable<Testimonial> lstBann = null;

            lstBann = db.Testimonials.Where(b => b.Status == true);
            List<TestimonialViewModel> lstModel = new List<TestimonialViewModel>();
            foreach (Testimonial obj in lstBann)
            {
                TestimonialViewModel model = new TestimonialViewModel();
                model.Image = obj.Image;
                lstModel.Add(model);
            }

            return lstModel;
        }
    }
}
