using GardenViewModel;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface IReportsRepository
    {
        IEnumerable<UserReportViewModel> GetUserData(string startDt, string EndDate);
        IEnumerable<CollectionViewModel> GetCollectionByDate(string startDt, string EndDate);
        IEnumerable<SaleProductViewModel> GetSaleProductByDate(string startDt, string EndDate);

        //IEnumerable<>
    }
    public class ReportsRepository : IReportsRepository
    {
        MyDBContext db = new MyDBContext();

        public IEnumerable<UserReportViewModel> GetUserData(string startDt, string EndDate)
        {
            IEnumerable<UserReportViewModel> lstModel = db.Database.SqlQuery<UserReportViewModel>(string.Format("Select Name, Email, ContactNo, Gender, Address from Users where RoleId=2 and CAST(CreatedDt AS DATE) between '{0}' and '{1}'", startDt, EndDate));
            return lstModel;
        }


        public IEnumerable<CollectionViewModel> GetCollectionByDate(string startDt, string EndDate)
        {
            IEnumerable<CollectionViewModel> lstModel = db.Database.SqlQuery<CollectionViewModel>(string.Format("Select OrderDt, TotalAmount, DeliveryCharges, DiscountAmt, FinalAmount, PayMode, PayRemark from GetOrderDetails where  CAST(OrderDt as DATE) between '{0}' and '{1}'", startDt, EndDate));
            return lstModel;
        }


        public IEnumerable<SaleProductViewModel> GetSaleProductByDate(string startDt, string EndDate)
        {
            IEnumerable<SaleProductViewModel> lstModel = db.Database.SqlQuery<SaleProductViewModel>(string.Format("SELECT Name, CAST(OrderDt AS DATE) as OrderDt, sum(Qty) Qty FROM GetSaleProducts where OrderDt between '{0}' and '{1}' group by CAST(OrderDt AS DATE), Name", startDt, EndDate));
            return lstModel;
        }
    }
}
