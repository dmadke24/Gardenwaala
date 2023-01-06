using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenViewModel;
using System.Net.Http;
using BAL;
using Gardenwaala.CustomSecurity;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class DashboardController : UserAdminBaseController
    {
        IDashboardRepository repoDash;
        IOrderRepository repoOrder;
        IUserRepository repoUser;

        public DashboardController()
        {
            repoDash = new DashboardRepository();
            repoOrder = new OrderRepository();
            repoUser = new UserRepository();
        }

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult DashboardCnt()
        {
            CountViewModel model = new CountViewModel();
            model = repoOrder.TodaysOrderCount();
            model.totalUrs = repoUser.GetUserCnt();
            return PartialView("_DashboardCnt",model);
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

    }
}