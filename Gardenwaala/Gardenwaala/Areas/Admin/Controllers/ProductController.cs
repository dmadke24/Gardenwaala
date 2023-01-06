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
    public class ProductController : UserAdminBaseController
    {

        IProductRepository repoProduct;
        ICategoryRepository repoCat;
        ISubCategoryRepository repoSubtype;


        public ProductController()
        {
            repoProduct = new ProductRepository();
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
            ProductGridViewModel model = null;

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


                model = repoProduct.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, searchStr.ToLower());

                return Json(new { draw = draw, recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.ProductList });
            }
            else
            {
                sortColumn = TempData["Sort"].ToString();
                sortDir = TempData["SortDir"].ToString();
                pageSize = Convert.ToInt32(TempData["PageSize"]);
                skip = Convert.ToInt32(TempData["Skip"]);
                model = repoProduct.GetAll(sortColumn.ToLower(), sortDir.ToLower(), skip, pageSize, null);
                return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.ProductList });
            }
        }

        public ViewResult Create()
        {
            ViewBag.TypeList = GetCategory();
            ViewBag.SubTypeList = GetSubCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {

            if (model.ImgFile1 == null)
                ModelState.AddModelError("ImgFile1", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImgFile1.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg1_" + timeStamp + ext;
                model.Image1 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile1.SaveAs(path);
            }

            if (model.ImgFile2 == null)
                ModelState.AddModelError("ImgFile2", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImgFile2.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg2_" + timeStamp + ext;
                model.Image2 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile2.SaveAs(path);
            }

            if (model.ImgFile3 == null)
                ModelState.AddModelError("ImgFile2", "Upload File.");
            else
            {
                string ext = Path.GetExtension(model.ImgFile3.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg3_" + timeStamp + ext;
                model.Image3 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile3.SaveAs(path);
            }

            if (repoProduct.Add(model))
                return RedirectToAction("Index");
            else
            {
                ViewBag.TypeList = GetCategory();
                ViewBag.SubTypeList = GetSubCategory();
                //    ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View();
            }
        }

        public ViewResult Edit(int id)
        {
            ViewBag.TypeList = GetCategory();
            ViewBag.SubTypeList = GetSubCategory();

            ProductViewModel model = repoProduct.GetById(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {

            if (model.ImgFile1 == null)
                model.Image1 = model.OldImage1;
            else
            {
                string ext = Path.GetExtension(model.ImgFile1.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg1_" + timeStamp + ext;
                model.Image1 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile1.SaveAs(path);
            }

            if (model.ImgFile2 == null)
                model.Image2 = model.OldImage2;
            else
            {
                string ext = Path.GetExtension(model.ImgFile2.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg2_" + timeStamp + ext;
                model.Image2 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile2.SaveAs(path);
            }

            if (model.ImgFile3 == null)
                model.Image3 = model.OldImage3;
            else
            {
                string ext = Path.GetExtension(model.ImgFile3.FileName);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string Filename = "fimg3_" + timeStamp + ext;
                model.Image3 = Filename;
                var path = Path.Combine(Server.MapPath("~/Areas/Admin/UploadImg/ProductImg/"), Filename);
                model.ImgFile3.SaveAs(path);
            }
            if (repoProduct.Update(model))
                return RedirectToAction("Index");
            else
            {
                ViewBag.TypeList = GetCategory();
                ViewBag.SubTypeList = GetSubCategory();
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return View("Create");
            }
        }

        public RedirectToRouteResult Delete(int id)
        {
            TempData["delStatus"] = false;
            if (repoProduct.Delete(id))
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

            repoProduct.ChangeStatus(Ids, Status);
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

        private IEnumerable<DropDownViewModel> GetCategory()
        {
            IEnumerable<DropDownViewModel> lstSubMenu = null;
            lstSubMenu = repoCat.GetCategoryList();
            return lstSubMenu;
        }

        private IEnumerable<DropDownViewModel> GetSubCategory()
        {
            IEnumerable<DropDownViewModel> lstSubMenu = null;
            lstSubMenu = repoSubtype.GetSubCategory();
            return lstSubMenu;
        }
    }
}