using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using GardenViewModel;
using BAL;
using Microsoft.Reporting.WebForms;

namespace Gardenwaala.Areas.Admin.Controllers.ShoppingCart
{
    public class OrderController : UserAdminBaseController
    {
        IOrderRepository repoOrder;

        public OrderController()
        {
            repoOrder = new OrderRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            string sortColumn, sortDir;
            int pageSize, skip;
            OrderGridViewModel model = null;

            if (TempData["FromGo"] == null)
            {
                //pagination parameters *****************************************
                //get page index(start) and page size(length)
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                //sort parameters ************************************************
                sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                sortDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                //search parameters
                var searchStr = Request.Form.GetValues("search[value]").FirstOrDefault();

                //pagination parameters
                pageSize = length != null ? Convert.ToInt32(length) : 0;
                skip = start != null ? Convert.ToInt32(start) : 0;


                model = repoOrder.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.OrderList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoOrder.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.OrderList });
            }
        }

        public ActionResult Go(bool Status, string chkIds, int skip, int pgSize, int sortColumn, string sortDir)
        {
            string[] allIds = chkIds.Split(',');
            int[] Ids = new int[allIds.Length];

            int ind = 0;
            foreach (string i in allIds)
                Ids[ind++] = Convert.ToInt32(i);

            TempData["FromGo"] = true;
            TempData["Skip"] = skip;
            TempData["PageSize"] = pgSize;

            switch (sortColumn)
            {
                case 1:
                    TempData["Sort"] = "btitle"; break;
            }

            TempData["SortDir"] = sortDir;
            return RedirectToAction("Index");
        }



        public ViewResult Details(long id)
        {
            OrderViewModel model = repoOrder.GetById(id);
            return View(model);
        }

        public RedirectToRouteResult Delete(long id)
        {
            TempData["delStatus"] = false;
            if (repoOrder.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }

        public ActionResult OrderStatus()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadOrderData()
        {
            string sortColumn, sortDir;
            int pageSize, skip;
            OrderGridViewModel model = null;

            if (TempData["FromGo"] == null)
            {
                //pagination parameters *****************************************
                //get page index(start) and page size(length)
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                //sort parameters ************************************************
                sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                sortDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                //search parameters
                var searchStr = Request.Form.GetValues("search[value]").FirstOrDefault();

                //pagination parameters
                pageSize = length != null ? Convert.ToInt32(length) : 0;
                skip = start != null ? Convert.ToInt32(start) : 0;


                model = repoOrder.GetAllStatusOrder(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.OrderList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoOrder.GetAllStatusOrder(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.OrderList });
            }
        }

        public ActionResult GoOrdStatus(string OrderStatus, string chkIds, int skip, int pgSize, int sortColumn, string sortDir)
        {
            string[] allIds = chkIds.Split(',');
            long[] Ids = new long[allIds.Length];

            int ind = 0;
            foreach (string i in allIds)
                Ids[ind++] = Convert.ToInt32(i);

            repoOrder.ChangeOrderStatus(Ids, OrderStatus);
            TempData["FromGo"] = true;
            TempData["Skip"] = skip;
            TempData["PageSize"] = pgSize;

            switch (sortColumn)
            {
                case 1:
                    TempData["Sort"] = "btitle"; break;
            }

            TempData["SortDir"] = sortDir;
            return RedirectToAction("OrderStatus");
        }

        public ActionResult GenerateBill(long id)
        {
            IEnumerable<BillDetailViewModel> lstDetail = repoOrder.GenerateBill(id);

            LocalReport lr = new LocalReport();
            string rptDataSet = "";
            lr.ReportPath = Server.MapPath("~/Reports/GenerateBill.rdlc");
            rptDataSet = "BillDataSet";

            ReportDataSource rdc = new ReportDataSource();
            rdc.Name = rptDataSet;
            rdc.Value = lstDetail;

            lr.DataSources.Add(rdc);
            string mimeType, encoding;
            string fext = "";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = lr.Render("pdf", "", out mimeType, out encoding, out fext, out streams, out warnings);

            System.IO.File.WriteAllBytes(Server.MapPath("~/BillPDF/" + id + "_Bill.pdf"), renderedBytes);
            return File(Server.MapPath("~/BillPDF/" + id + "_Bill.pdf"), "application/pdf");
        }

    }
}