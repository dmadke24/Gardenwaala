using BAL;
using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class AreaController : UserAdminBaseController
    {
        IAreaRepository repoArea;
        ICityRepository repoCity;

        public AreaController()
        {
            repoArea = new AreaRepository();
            repoCity = new CityRepository();
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

                AreaGridViewModel model = repoArea.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.AreaList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                AreaGridViewModel model = repoArea.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.AreaList });

            }
        }
        public ViewResult Create()
        {
            ViewBag.CityList = repoCity.GetCities();
            return View();
        }
        [HttpPost]
        public ActionResult Create(AreaViewModel model)
        {
            if (ModelState.IsValid && repoArea.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                ViewBag.CityList = repoCity.GetCities();
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            ViewBag.CityList = repoCity.GetCities();
            AreaViewModel model = repoArea.GetById(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(AreaViewModel model)
        {
            if (ModelState.IsValid && repoArea.Update(model))
                return RedirectToAction("Index");
            else
            {
                ViewBag.CityList = repoCity.GetCities();
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View("Create");
            }
        }

        public RedirectToRouteResult Delete(int id)
        {
            TempData["delStatus"] = false;
            if (repoArea.Delete(id))
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

            repoArea.ChangeStatus(Ids, Status);
            TempData["FromGo"] = true;
            TempData["Skip"] = skip;
            TempData["PageSize"] = pgSize;

            switch (sortColumn)
            {
                case 1:
                    TempData["Sort"] = "aname"; break;
                case 2:
                    TempData["Sort"] = "cityname"; break;
            }

            TempData["SortDir"] = sortDir;
            return RedirectToAction("Index");
        }
    }
}