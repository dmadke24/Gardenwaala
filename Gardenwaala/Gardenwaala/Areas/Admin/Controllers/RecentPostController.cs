using GardenViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BAL;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class RecentPostController : UserAdminBaseController
    {
        IRecentUpdateRepository repoRUpdate;
        public RecentPostController()
        {
            repoRUpdate = new RecentUpdateRepository();
        }

        // GET: UserAdmin/Banner
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            string sortColumn, sortDir;
            int pageSize, skip;
            RecentUpdateGridViewModel model = null;

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


                model = repoRUpdate.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.RecentUpdateList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoRUpdate.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.RecentUpdateList });
            }

        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RecentUpdateViewModel model)
        {
            //explicit validation for check boxes of post media
            if (model.ImageFile != null)
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }

            //explicit validation for check boxes of post media
            if (model.ImageFile1 != null)
            {
                string ext = Path.GetExtension(model.ImageFile1.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg1_" + timeStamp + ext;
                model.Image1 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile1.SaveAs(path);
            }

            //explicit validation for check boxes of post media
            if (model.ImageFile2 != null)
            {
                string ext = Path.GetExtension(model.ImageFile2.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg2_" + timeStamp + ext;
                model.Image2 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile2.SaveAs(path);
            }

            if (repoRUpdate.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            RecentUpdateViewModel model = repoRUpdate.GetById(id);
            return View("Create", model);
        }


        [HttpPost]
        public ActionResult Edit(RecentUpdateViewModel model) //Transfer content from model to webapimodel
        {

            if (model.ImageFile == null)
                model.Image = model.OldFile;
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }

            if (model.ImageFile1 == null)
                model.Image1 = model.OldFile1;
            else
            {
                string ext = Path.GetExtension(model.ImageFile1.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg1_" + timeStamp + ext;
                model.Image1 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile1.SaveAs(path);
            }

            if (model.ImageFile2 == null)
                model.Image2 = model.OldFile;
            else
            {
                string ext = Path.GetExtension(model.ImageFile2.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "RecentPostImg2_" + timeStamp + ext;
                model.Image2 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/RecentPostImg/"), Filename);
                model.ImageFile2.SaveAs(path);
            }
            if (repoRUpdate.Update(model))
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
            if (repoRUpdate.Delete(id))
                TempData["delStatus"] = true;
            return RedirectToAction("Index");
        }
    }
}