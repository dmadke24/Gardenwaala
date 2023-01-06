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
    public class CRMEnquiryController : UserAdminBaseController
    {
        IEnquiryRepository repoEnq;
        public CRMEnquiryController()
        {
            repoEnq = new EnquiryRepository();
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
            EnquiryGridViewModel model = null;

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


                model = repoEnq.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.EnquiryList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoEnq.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);

                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.EnquiryList });
            }
        }


        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EnquiryViewModel model)
        {
            model.EnqDt = DateTime.Now;
            if (repoEnq.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }


        public ViewResult Edit(int id)
        {
            EnquiryViewModel model = repoEnq.GetById(id);
            return View("Create", model);
        }


        [HttpPost]
        public ActionResult Edit(EnquiryViewModel model)
        {
            model.EnqDt = DateTime.Now;
            if (repoEnq.Update(model))
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
            if (repoEnq.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }

        public FileResult Download()
        {
            return File("~/Areas/Admin/csvFormat/EnquiryFormat.csv", "text/csv", "EnquiryFormat.csv");
        }

    }
}