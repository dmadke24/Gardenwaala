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
    public class EpamphletController : UserAdminBaseController
    {
        IEpamphletRepository repoEpmp;
        public EpamphletController()
        {
            repoEpmp = new EpamphletRepository();
        }

        // GET: UserAdmin/Epamphlet
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            string sortColumn, sortDir;
            int pageSize, skip;
            EpamphletGridViewModel model = null;

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


                model = repoEpmp.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.EpamphletList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoEpmp.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.EpamphletList });
            }
        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(EpamphletViewModel model)
        {
            if (model.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImageFile.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "EpamphletImg_" + timeStamp + ext;
                model.Image = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/EpamphletImg/"), Filename);
                model.ImageFile.SaveAs(path);
            }
            if (repoEpmp.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
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
    }
}