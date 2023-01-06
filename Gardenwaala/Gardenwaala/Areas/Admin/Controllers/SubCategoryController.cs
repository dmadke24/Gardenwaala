using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BAL;
using GardenViewModel;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class SubCategoryController : UserAdminBaseController
    {
        ISubCategoryRepository repoSubtype;
        ICategoryRepository repoCat;
        public SubCategoryController()
        {
            repoCat = new CategoryRepository();
            repoSubtype = new SubCategoryRepository();
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
            SubCategoryGridViewModel model = null;

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


                model = repoSubtype.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.SubCategoryList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoSubtype.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.SubCategoryList });
            }
        }

        public ViewResult Create() //To display a create view
        {
            ViewBag.TypeList = GetCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Create(SubCategoryViewModel model) //on Submit
        {

            if (model.ImageFile != null)
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "CategoryImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/SubCategoryImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoSubtype.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                ViewBag.TypeList = GetCategory();
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            ViewBag.TypeList = GetCategory();
            SubCategoryViewModel model = repoSubtype.GetById(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(SubCategoryViewModel model)//on submit
        {
            if (model.ImageFile == null)
                model.Image = model.OldFile;
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "CategoryImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/SubCategoryImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoSubtype.Update(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                ViewBag.TypeList = GetCategory();
                return View("Create");
            }
        }

        public RedirectToRouteResult Delete(int id)
        {
            TempData["delStatus"] = false;
            if (repoSubtype.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }

        private IEnumerable<DropDownViewModel> GetCategory()
        {
            IEnumerable<DropDownViewModel> lstSubMenu = null;
            lstSubMenu = repoCat.GetCategoryList();
            return lstSubMenu;
        }
    }
}