using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.IO;
using BAL;
using GardenViewModel;

namespace Gardenwaala.Areas.Admin.Controllers
{

    public class BannerController : UserAdminBaseController
    {
        IBannerRepository repoBan;

        public BannerController()
        {
            repoBan = new BannerRepository();
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
            BannerGridViewModel model = null;

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


                model = repoBan.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.BannerList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoBan.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.BannerList });
            }
        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(BannerViewModel model)
        {
            if (model.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "BannerImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/BannerImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoBan.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            BannerViewModel model = repoBan.GetById(id);
            return View("Create", model);
        }


        [HttpPost]
        public ActionResult Edit(BannerViewModel model) //Transfer content from model to webapimodel
        {

            if (model.ImageFile == null)
                model.Image = model.OldFile;
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "BannerImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/BannerImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoBan.Update(model))
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
            if (repoBan.Delete(id))
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

            repoBan.ChangeStatus(Ids, Status);
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
