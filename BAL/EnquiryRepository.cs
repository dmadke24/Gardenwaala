using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using GardenViewModel;

namespace BAL
{
    public interface IEnquiryRepository
    {
        bool Add(EnquiryViewModel model);
        bool Update(EnquiryViewModel model);
        bool Delete(int id);
        EnquiryViewModel GetById(int id);
        EnquiryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);

        IEnumerable<string> GetContacts();
        IEnumerable<string> GetEmails();
        bool AddNewsEmail(string emailId); // Add Newsletter email Id
    }

    public class EnquiryRepository : IEnquiryRepository
    {
        MyDBContext db = new MyDBContext();

        //Generating new Id Menually
        private int NewId()
        {
            string oldid = Convert.ToString(db.Enquiries.Select(e => e.EnquiryId)
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

        public bool Add(EnquiryViewModel model)
        {
            try
            {
                Enquiry obj = new Enquiry();
                obj.EnquiryId = NewId(); //Id generated menually
                obj.EnqDt = model.EnqDt;
                obj.Name = model.Name;
                obj.EmailId = model.EmailId;
                obj.ContactNo = model.ContactNo;
                obj.AlternatContactNo = model.AlternatContactNo;
                obj.Description = model.Description;

                obj.Subject = model.Subject;

                //Add to DB
                db.Enquiries.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch(DbEntityValidationException ex)
            {

                return false;
            }
        }

        public bool Update(EnquiryViewModel model)
        {
            try
            {
                SqlParameter paramAltContact = new SqlParameter();
                paramAltContact.ParameterName = "AlternatContactNo";
                if (!string.IsNullOrEmpty(model.AlternatContactNo))
                    paramAltContact.Value = model.AlternatContactNo;
                else
                    paramAltContact.Value = DBNull.Value;


                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "EmailId";
                if (!string.IsNullOrEmpty(model.EmailId))
                    paramEmail.Value = model.EmailId;
                else
                    paramEmail.Value = DBNull.Value;


                db.Database.ExecuteSqlCommand("Exec UpdateManualEnquiry @EnquiryId,@Enqdt, @Name, @EmailId, @ContactNo, @AlternatContactNo, @Description,@Subject",
                        new SqlParameter("EnquiryId", model.EnquiryId),
                        new SqlParameter("Enqdt", model.EnqDt),
                        new SqlParameter("Name", model.Name),
                        paramEmail,
                        new SqlParameter("ContactNo", model.ContactNo),
                        paramAltContact,
                        new SqlParameter("Description", model.Description),
                        new SqlParameter("Subject", model.Subject));
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
                Enquiry obj = db.Enquiries.Find(id);
                if (obj != null)
                {
                    db.Enquiries.Remove(obj);
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

        public EnquiryViewModel GetById(int id)
        {
            try
            {
                EnquiryViewModel model = new EnquiryViewModel();
                Enquiry obj = db.Enquiries.Find(id);
                if (obj != null)
                {
                    model.EnquiryId = obj.EnquiryId;
                    model.EnqDt = obj.EnqDt;
                    model.Name = obj.Name;
                    model.EmailId = obj.EmailId;
                    model.ContactNo = obj.ContactNo;
                    model.AlternatContactNo = obj.AlternatContactNo;
                    model.Description = obj.Description;
                    model.Subject = obj.Subject;
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

        public EnquiryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            EnquiryGridViewModel gridModel = new EnquiryGridViewModel();

            IQueryable<Enquiry> lstEnquiry = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstEnquiry = db.Enquiries;
            }
            else
            {
                lstEnquiry = db.Enquiries.Where(e => (e.Name.ToLower().Contains(searchStr) || e.Subject.ToLower().Contains(searchStr) || e.ContactNo.Contains(searchStr) || e.AlternatContactNo.Contains(searchStr)));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "name":
                    lstEnquiry = sortDir == "asc" ? lstEnquiry.OrderBy(e => e.Name) : lstEnquiry.OrderByDescending(e => e.Name);
                    break;

                case "enqdt":
                    lstEnquiry = sortDir == "asc" ? lstEnquiry.OrderBy(e => e.EnqDt) : lstEnquiry.OrderByDescending(e => e.EnqDt);
                    break;
                default:
                    lstEnquiry = lstEnquiry.OrderByDescending(e => e.EnquiryId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstEnquiry.Count();

            //pagination code start******************************************************
            lstEnquiry = lstEnquiry.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<EnquiryViewModel> lstModel = new List<EnquiryViewModel>();

            foreach (Enquiry obj in lstEnquiry.ToList<Enquiry>())
            {
                EnquiryViewModel model = new EnquiryViewModel();

                model.EnquiryId = obj.EnquiryId;
                model.EnqDt = obj.EnqDt;
                model.EnqDisplayDt = obj.EnqDt.ToString("dd-MMM-yyyy");
                model.Name = obj.Name;
                model.EmailId = obj.EmailId;
                model.ContactNo = obj.ContactNo;
                model.AlternatContactNo = obj.AlternatContactNo;
                model.Description = obj.Description;
                model.Subject = obj.Subject;
                lstModel.Add(model);
            }

            gridModel.EnquiryList = lstModel;
            return gridModel;
        }


        public IEnumerable<string> GetContacts()
        {
            IEnumerable<string> lstModel = db.Database.SqlQuery<string>("Select ContactNo From Enquiries Where ContactNo <> 'N/A' AND ContactNo <> '' AND ContactNo Is Not NULL");

            return lstModel;
        }

        public IEnumerable<string> GetEmails()
        {

            IEnumerable<string> lstModel = db.Database.SqlQuery<string>("Select EmailId From Enquiries Where EmailId <>'' AND EmailId Is NOT NULL");
            return lstModel;
        }



        public bool AddNewsEmail(string emailId)
        {
            try
            {
                Enquiry obj = new Enquiry();
                obj.EnquiryId = NewId(); //Id generated menually
                obj.EnqDt = DateTime.Now;
                obj.Name = "Newsletter User";
                obj.EmailId = emailId;
                obj.ContactNo = "9999999999";
                obj.AlternatContactNo = "";
                obj.Description = "Client wants updates of Newsletters";

                obj.Subject = "Apply for Newsletter";

                //Add to DB
                db.Enquiries.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {

                return false;
            }
        }
    }
}
