using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BAL;
using GardenViewModel;

namespace Gardenwaala.Areas.Admin.Controllers.DiscountTax
{
    public class DiscountCouponController : UserAdminBaseController
    {
        IDiscountCouponRepository repoDisc;
        ICategoryRepository repoCat;

        public DiscountCouponController()
        {
            repoDisc = new DiscountCouponRepository();
            repoCat = new CategoryRepository();
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
            DiscountCouponGridViewModel model = null;

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


                model = repoDisc.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CouponList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoDisc.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CouponList });
            }

        }

        public ViewResult Create() //To display a create view
        {
            ViewBag.TypeList = GetCategory();
            return View();
        }

        private IEnumerable<DropDownViewModel> GetCategory()
        {
            IEnumerable<DropDownViewModel> lstSubMenu = null;
            lstSubMenu = repoCat.GetCategoryList();
            return lstSubMenu;
        }

        [HttpPost]
        public ActionResult Create(DiscountCouponViewModel model) //on Submit
        {

            if (repoDisc.Add(model))
                return RedirectToAction("Index");
            else
            {
                ViewBag.TypeList = GetCategory();
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }

        public ViewResult Edit(int id) //  on Edit hyperlink to display a selected record
        {

            DiscountCouponViewModel model = repoDisc.GetById(id);
            ViewBag.TypeList = GetCategory();
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(DiscountCouponViewModel model)//on submit
        {

            if (repoDisc.Update(model))
                return RedirectToAction("Index");
            else
            {
                ViewBag.TypeList = GetCategory();
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View("Create");
            }
        }

        public RedirectToRouteResult Delete(int id)
        {
            TempData["delStatus"] = false;
            if (repoDisc.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }
    }
}