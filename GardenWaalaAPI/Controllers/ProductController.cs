using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL;
using GardenWaalaAPI.Security;
using GardenViewModel;
using GardenAPIViewModel;

namespace GardenWaalaAPI.Controllers
{
    [RoutePrefix("GardenWaalaAPI/ProductInfo")]
    public class ProductController : ApiController
    {
        IProductRepository repoProd;
        ICategoryRepository repoCategory;

        public ProductController()
        {
            repoProd = new ProductRepository();
            repoCategory = new CategoryRepository();
        }

        [Route("GetProductCategory")]
        [HttpGet]
        public IHttpActionResult GetProductCategory()
        {
            CategoryAPIViewModel apiModel = new CategoryAPIViewModel();
            IEnumerable<CategoryViewModel> model = repoCategory.GetCategory();
            if (model != null)
            {
                apiModel.msg = "Category Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/CategoryImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/CategoryImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("GetProductSubCategory/{cid:int}")]
        [HttpGet]
        public IHttpActionResult GetProductSubCategory(int cid)
        {
            SubCategoryAPIViewModel apiModel = new SubCategoryAPIViewModel();
            ISubCategoryRepository repoSubtype = new SubCategoryRepository();
            IEnumerable<SubCategoryViewModel> lststype = repoSubtype.GetSubCategoryMenu(cid);
            if (lststype != null)
            {
                apiModel.msg = "Sup Category Data fetched Successfully.";
                apiModel.status = true;
                apiModel.data = lststype;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("GetBestSeller")]
        [HttpGet]
        public IHttpActionResult GetBestSeller()
        {
            ProductListAPIViewModel apiModel = new ProductListAPIViewModel();
            IEnumerable<ProductViewModel> model = repoProd.GetBestSeller();
            if (model != null)
            {
                apiModel.msg = "Best Seller Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("GetHomeProduct/{pid:int}/{subid:int}")]
        [HttpGet]
        public IHttpActionResult GetHomeProduct(int pid, int subid)
        {
            ProductListAPIViewModel apiModel = new ProductListAPIViewModel();
            IEnumerable<ProductViewModel> model = repoProd.GetHomeProduct(pid, subid);
            if (model != null)
            {
                apiModel.msg = "Home Products Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("GetRelatedProduct/{pid:int}")]
        [HttpGet]
        public IHttpActionResult GetRelatedProduct(int pid)
        {
            ProductListAPIViewModel apiModel = new ProductListAPIViewModel();
            IEnumerable<ProductViewModel> model = repoProd.GetRelatedProduct(pid);
            if (model != null)
            {
                apiModel.msg = "Related Product Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }

        }

        [Route("GetSubProductlist/{pid:int}/{subid:int}")]
        [HttpGet]
        public IHttpActionResult GetSubProductlist(int pid, int subid = 0)
        {
            ProductListAPIViewModel apiModel = new ProductListAPIViewModel();
            IEnumerable<ProductViewModel> lstModel = repoProd.GetHomeProduct(pid, subid);
            if (lstModel != null)
            {
                apiModel.msg = "Category Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = lstModel;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("GetProductDetail/{pid:int}")]
        [HttpGet]
        public IHttpActionResult GetProductDetail(int pid)
        {
            ProductDetailAPIViewModel apiModel = new ProductDetailAPIViewModel();
            ProductViewModel model = repoProd.GetDetailProduct(pid);
            if (model != null)
            {
                apiModel.msg = "Category Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("searchproduct/{SearchData}")]
        [HttpGet]
        public IHttpActionResult GetProductDetail(string SearchData)
        {
            ProductListAPIViewModel apiModel = new ProductListAPIViewModel();
            IEnumerable<ProductViewModel> model = repoProd.GetSearchData(SearchData);
            if (model != null)
            {
                apiModel.msg = "Category Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg";
                apiModel.status = true;
                apiModel.data = model;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "No Data Available.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }
    }
}