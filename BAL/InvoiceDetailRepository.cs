using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;
using System.Data.Entity;

namespace CRMBAL.Invoice
{
    public interface IInvoiceDetailRepository
    {
        bool Add(InvoiceDetailViewModel model);
        bool Delete(long id);
        IEnumerable<InvoiceDetailViewModel> GetById(long id); // Get the details list by invoice id
        IEnumerable<InvoiceDetailViewModel> DeleteOrder(long invid, long invdetid); // Get the details list by invoice id
        bool UpdateDish(long invdetid, int dishCnt);
        //    IEnumerable<InvoiceDetailViewModel> GetOrderDetail(int id);
    }

    public class InvoiceDetailRepository : IInvoiceDetailRepository
    {
        MyDBContext db = new MyDBContext();
        private long NewId()
        {
            string oldid = Convert.ToString(db.InvoiceDetails.Select(i => i.InvoiceDetailId).DefaultIfEmpty().Max());

            string myid, newid;
            int year = Convert.ToInt32(DateTime.Now.ToString("yy"));
            if (oldid == "0")
            {
                myid = year + "00000000001";
                return Convert.ToInt64(myid);
            }
            else
            {
                //Split the id into two parts year and no
                string old_year = oldid.Substring(0, 2);
                long old_no = Convert.ToInt64(oldid.Substring(2));

                if (year == Convert.ToInt32(old_year))
                {
                    old_no += 1;
                    newid = old_year + old_no.ToString("00000000000");
                }
                else
                    newid = year + "00000000001";

                return (Convert.ToInt64(newid));
            }
        }

        public bool Add(InvoiceDetailViewModel model)
        {
            try
            {
                InvoiceDetail obj = db.InvoiceDetails.Where(id => id.Particulars == model.Particulars && id.InvoiceId == model.InvoiceId).FirstOrDefault();
                if (obj == null)
                {
                    obj = new InvoiceDetail();
                    obj.InvoiceDetailId = NewId();
                    obj.InvoiceId = model.InvoiceId;
                    obj.Particulars = model.Particulars;
                    obj.Price = model.Price;
                    obj.Qty = model.Qty;
                    db.InvoiceDetails.Add(obj);
                }
                else
                {
                    obj.Qty = obj.Qty + model.Qty;
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                InvoiceDetail obj = db.InvoiceDetails.Find(id);

                if (obj != null)
                {
                    db.InvoiceDetails.Remove(obj);
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


        public IEnumerable<InvoiceDetailViewModel> GetById(long id)
        {
            IEnumerable<InvoiceDetailViewModel> lstDetail = db.Database.SqlQuery<InvoiceDetailViewModel>("Select * from InvoiceDetails where InvoiceId=" + id);
            return lstDetail;
        }

        public IEnumerable<InvoiceDetailViewModel> DeleteOrder(long invid, long invdetid)
        {
            if (Delete(invdetid))
            {
                IEnumerable<InvoiceDetailViewModel> lstDetail = db.Database.SqlQuery<InvoiceDetailViewModel>("Select * from InvoiceDetails where InvoiceId=" + invid);
                return lstDetail;
            }
            else
            {
                IEnumerable<InvoiceDetailViewModel> lstDetail = db.Database.SqlQuery<InvoiceDetailViewModel>("Select * from InvoiceDetails where InvoiceId=" + invid);
                return lstDetail;
            }
        }


        public bool UpdateDish(long invdetid, int dishCnt)
        {
            try
            {
                InvoiceDetail obj = db.InvoiceDetails.Find(invdetid);

                if (obj != null)
                {
                    obj.Qty = dishCnt;
                    db.Entry<InvoiceDetail>(obj).State = EntityState.Modified;
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
    }
}