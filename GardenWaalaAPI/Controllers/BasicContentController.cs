using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL;
using GardenAPIViewModel;
using GardenViewModel;

namespace GardenWaalaAPI.Controllers
{
    [RoutePrefix("GardenWaalaAPI/BasicContent")]
    public class BasicContentController : ApiController
    {
        IBannerRepository repoBan;
        IFAQRepository repoFAQ;
        IDiscountCouponRepository repoDiscount;

        public BasicContentController()
        {
            repoBan = new BannerRepository();
            repoFAQ = new FAQRepository();
            repoDiscount = new DiscountCouponRepository();
        }

        [Route("GetBanner")]
        [HttpGet]
        public IHttpActionResult GetBanner()
        {
            BannerAPIViewModel apiModel = new BannerAPIViewModel();

            IEnumerable<BannerViewModel> model = repoBan.GetHomeBanner();
            if (model != null)
            {
                apiModel.msg = "Data fetched Successfully.";
                apiModel.ImageURL = "https://gardenwaala.com/Areas/Admin/UploadImg/BannerImg";
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

        [Route("GetAboutUs")]
        [HttpGet]
        public IHttpActionResult GetAboutUs()
        {
            IEnumerable<BannerViewModel> model = repoBan.GetHomeBanner();
            return Ok(model);
        }

        [Route("GetFAQ")]
        [HttpGet]
        public IHttpActionResult GetFAQ()
        {
            FAQAPIViewMOdel apiModel = new FAQAPIViewMOdel();
            IEnumerable<FAQViewModel> model = repoFAQ.GetHomeFAQ();

            if (model != null)
            {
                apiModel.msg = "Data fetched Successfully.";
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

        [Route("GetDiscountCoupon")]
        [HttpGet]
        public IHttpActionResult GetDiscountCoupon()
        {
            DiscountCouponAPIViewMOdel apiModel = new DiscountCouponAPIViewMOdel();
            IEnumerable<DiscountCouponViewModel> model = repoDiscount.HomeCouponImage();

            if (model != null)
            {
                apiModel.msg = "Data fetched Successfully.";
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

        [Route("ValidateCoupon/{CouponCode}")]
        [HttpGet]
        public IHttpActionResult ValidateCoupon(string CouponCode)
        {
            ValidateCouponAPIViewMOdel apiModel = new ValidateCouponAPIViewMOdel();
            float DiscPercent = repoDiscount.ValidateCoupon(CouponCode);
            DiscountCouponPercetgViewModel model = new DiscountCouponPercetgViewModel();

            if (DiscPercent != 0.0)
            {
                model.DiscPercent = DiscPercent;
                apiModel.msg = "Data fetched Successfully.";
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
