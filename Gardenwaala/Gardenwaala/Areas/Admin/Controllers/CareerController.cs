using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.IO;
using GardenViewModel;
using BAL;


namespace Gardenwaala.Areas.Admin.Controllers
{
    public class CareerController : UserAdminBaseController
    {

        ICareerRepository repoCar;

        public CareerController()
        {
            repoCar = new CareerRepository();
        }

        // GET: UserAdmin/Career
        public ActionResult Index() //To display a index view
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            string sortColumn, sortDir;
            int pageSize, skip;
            CareerGridViewModel model = null;

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

                model = repoCar.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CareerList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoCar.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CareerList });

            }
        }

        public ViewResult Create() //To display a create view
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CareerViewModel model) //on Submit
        {

            if (ModelState.IsValid)
            {

                repoCar.Add(model);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }
        }

        public ViewResult Edit(int id) //  on Edit hyperlink to display a selected record
        {
            CareerViewModel model = repoCar.GetById(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(CareerViewModel model)//on submit
        {

            if (repoCar.Update(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View("Create");
            }
        }

        public RedirectToRouteResult Delete(int id)
        {
            TempData["delStatus"] = false;
            if (repoCar.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }

        public ActionResult Go(bool Status, string chkIds, int skip, int pgSize, int sortColumn, string sortDir)
        {
            string[] allIds = chkIds.Split(',');
            int[] Ids = new int[allIds.Length];

            int ind = 0;
            foreach (string i in allIds)
                Ids[ind++] = Convert.ToInt32(i);

            repoCar.ChangeStatus(Ids, Status);
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

    }
}