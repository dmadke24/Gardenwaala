using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using ClosedXML.Excel;
using GardenViewModel;
using Microsoft.Reporting.WebForms;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class ReportsController : UserAdminBaseController
    {
        IReportsRepository repoReport;
        public ReportsController()
        {
            repoReport = new ReportsRepository();
        }

        // GET: User/Reports
        public ActionResult UserData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserData(FormCollection collection)
        {
            string startDt = collection["startDt"];
            string endDt = collection["endDt"];
            IEnumerable<UserReportViewModel> lstDetail = null;

            if (startDt == null || endDt == null || startDt == "" || endDt == "")
                lstDetail = repoReport.GetUserData(DateTime.Now.Date.ToString(), DateTime.Now.Date.ToString());
            else
                lstDetail = repoReport.GetUserData(startDt, endDt);

            LocalReport lr = new LocalReport();
            string rptDataSet = "";
            lr.ReportPath = Server.MapPath("~/Reports/UserDetails.rdlc");
            rptDataSet = "DSUserDetails";

            ReportDataSource rdc = new ReportDataSource();
            rdc.Name = rptDataSet;
            rdc.Value = lstDetail;

            lr.DataSources.Add(rdc);
            string mimeType, encoding;
            string fext = "xlsx";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render("excel", "", out mimeType, out encoding, out fext, out streams, out warnings);

            System.IO.File.WriteAllBytes(Server.MapPath("~/ReportingFiles/UsersDetails.xlsx"), renderedBytes);

            return File(Server.MapPath("~/ReportingFiles/UsersDetails.xlsx"), "application/xlsx", "UsersDetails." + fext);
        }

        // GET: Collection/Reports
        public ActionResult Collection()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Collection(FormCollection collection)
        {
            string startDt = collection["startDt"];
            string endDt = collection["endDt"];
            IEnumerable<CollectionViewModel> lstDetail = repoReport.GetCollectionByDate(startDt, endDt);

            IXLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Collection details");
            ws.Cell(1, 1).Value = "Collection Details From " + startDt + " To " + endDt;
            ws.Cell(2, 1).Value = "Sr.No";
            ws.Cell(2, 2).Value = "Order Dt";
            ws.Cell(2, 3).Value = "Total Amount";
            ws.Cell(2, 4).Value = "Delivery Charges";
            ws.Cell(2, 5).Value = "Discount";
            ws.Cell(2, 6).Value = "Final Amount";
            ws.Cell(2, 7).Value = "Pay Mode";
            ws.Cell(2, 8).Value = "Pay Remark";

            int j = 3; int cnt = 1;
            foreach (var dmcntdata in lstDetail)
            {
                ws.Cell(j, 1).Value = cnt;
                ws.Cell(j, 2).Value = dmcntdata.OrderDt;
                ws.Cell(j, 3).Value = dmcntdata.TotalAmount.ToString("0.00");
                ws.Cell(j, 4).Value = dmcntdata.DeliveryCharges.ToString("0.00");
                ws.Cell(j, 5).Value = dmcntdata.DiscountAmt.ToString("0.00");
                ws.Cell(j, 6).Value = dmcntdata.FinalAmount.ToString("0.00");
                ws.Cell(j, 7).Value = dmcntdata.PayMode;
                ws.Cell(j, 8).Value = dmcntdata.PayRemark;

                j++; cnt++;
            }

            wb.SaveAs(Server.MapPath("~/ReportingFiles/CollectionDetalis.xlsx"));

            return File(Server.MapPath("~/ReportingFiles/CollectionDetalis.xlsx"), "application/xlsx", "CollectionDetalis.xlsx");

        }

        // GET: Daily Sale Products/Reports
        public ActionResult DailySaleProducts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DailySaleProducts(FormCollection collection)
        {
            string startDt = collection["startDt"];
            string endDt = collection["endDt"];
            IEnumerable<SaleProductViewModel> lstDetail = repoReport.GetSaleProductByDate(startDt, endDt);

            IXLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Sale Products details");
            ws.Cell(1, 1).Value = "Sale Products Details From " + startDt + " To " + endDt;
            ws.Cell(2, 1).Value = "Sr.No";
            ws.Cell(2, 2).Value = "Product Name";
            ws.Cell(2, 3).Value = "Order Date";
            ws.Cell(2, 4).Value = "Quantity";

            int j = 3; int cnt = 1;
            foreach (var dmcntdata in lstDetail)
            {
                ws.Cell(j, 1).Value = cnt;
                ws.Cell(j, 2).Value = dmcntdata.Name;
                ws.Cell(j, 3).Value = dmcntdata.OrderDt;
                ws.Cell(j, 4).Value = dmcntdata.Qty;
                j++; cnt++;
            }

            wb.SaveAs(Server.MapPath("~/ReportingFiles/SaleProducts.xlsx"));

            return File(Server.MapPath("~/ReportingFiles/SaleProducts.xlsx"), "application/xlsx", "SaleProducts.xlsx");

        }
    }
}