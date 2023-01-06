using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;
namespace BAL
{
    public interface ITaxRepository
    {
        bool Add(TaxViewModel model);
        TaxViewModel GetTaxDet();
    }
    public class TaxRepository : ITaxRepository
    {
        MyDBContext db = new MyDBContext();

        public bool Add(TaxViewModel model)
        {
            try
            {
                Tax obj = db.Taxes.FirstOrDefault();
                bool existFlag = true;
                if (obj == null)
                {
                    obj = new Tax();
                    existFlag = false;
                }
                obj.CGST = model.CGST;
                obj.SGST = model.SGST;
                obj.VAT = model.VAT;
                obj.ServiceTax = model.ServiceTax;

                if (!existFlag)
                    db.Taxes.Add(obj);

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TaxViewModel GetTaxDet()
        {
            TaxViewModel model = null;
            Tax obj = db.Taxes.FirstOrDefault();
            if (obj != null)
            {
                model = new TaxViewModel();
                model.CGST = obj.CGST;
                model.SGST = obj.SGST;
                model.VAT = obj.VAT;
                model.ServiceTax = obj.ServiceTax;
            }
            return model;
        }
    }
}
