using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BAL;
using GardenViewModel;
using System.IO;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class CategoryController : UserAdminBaseController
    {
        ICategoryRepository repoCat;
        public CategoryController()
        {
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
            CategoryGridViewModel model = null;

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


                model = repoCat.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CategoryList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoCat.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.CategoryList });
            }
        }

        public ViewResult Create() //To display a create view
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel model) //on Submit
        {
            if (model.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "CategoryImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/CategoryImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoCat.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            CategoryViewModel model = repoCat.GetById(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)//on submit
        {
            if (model.ImageFile == null)
                model.Image = model.OldFile;
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "CategoryImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/CategoryImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoCat.Update(model))
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
            if (repoCat.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }
    }
}