using BAL;
using Gardenwaala.Areas.Client.Models;
using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gardenwaala.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        // GET: Client/Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public PartialViewResult HeaderMenu(int pid = 0)
        {
            ICategoryRepository repoCat = new CategoryRepository();
            IEnumerable<CategoryViewModel> lstFtype = repoCat.GetCategory();
            ViewBag.CurrentMenuid = pid;
            return PartialView("_HeaderMenu", lstFtype);
        }

        public PartialViewResult CategoryMenu()
        {
            ICategoryRepository repoCat = new CategoryRepository();
            IEnumerable<CategoryViewModel> lstCat = repoCat.GetCategory();
            return PartialView("_CategoryMenu", lstCat);
        }

        public PartialViewResult SubCategory(int cid)
        {
            ISubCategoryRepository reposCat = new SubCategoryRepository();
            IEnumerable<SubCategoryViewModel> lstSCat = reposCat.GetSubCategoryMenu(cid);
            return PartialView("_SubCategory", lstSCat);
        }

        //Display sub category 8 products
        public PartialViewResult SubCatProduct(int scid)
        {
            IProductRepository repoProd = new ProductRepository();
            IEnumerable<ProductViewModel> lstSCat = repoProd.GetProductBySubCat(scid);
            return PartialView("_SubCatProduct", lstSCat);
        }

        public PartialViewResult HeaderOffers()
        {
            IDiscountCouponRepository repoDisc = new DiscountCouponRepository();
            IEnumerable<DiscountCouponViewModel> lstCoupon = repoDisc.HomeCouponImage();
            return PartialView("_HeaderOffers", lstCoupon);
        }

        public PartialViewResult Banner()
        {
            IBannerRepository repoBann = new BannerRepository();
            IEnumerable<BannerViewModel> lstModel = repoBann.GetHomeBanner();
            return PartialView("_Banner", lstModel);
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public PartialViewResult HomeSliderProd()
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetHomeSlideProducts();
            return PartialView("_HomeSliderProd", lstModel);
        }

        public PartialViewResult BestSeller()
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetBestSeller();
            return PartialView("_BestSeller", lstModel);
        }

        public PartialViewResult Testimonial()
        {
            ITestimonialRepository repoTest = new TestimonialRepository();
            IEnumerable<TestimonialViewModel> lstModel = repoTest.GetHomeTestimonial();
            return PartialView("_Testimonial", lstModel);
        }

        public PartialViewResult Newsletter()
        {
            return PartialView("_Newsletter");
        }

        [HttpPost]
        public JsonResult NewsSubscribe(OnlyEmailViewModel nmodel)
        {
            IEnquiryRepository repoEnq = new EnquiryRepository();
            if (repoEnq.AddNewsEmail(nmodel.Email))
            {
                //EMAIL to CLIENT for subscribibg us
                string emailMsg = "Thank you for joining <a href='https://gardenwaala.com' target='_blank'>Gardenwaala.com</a>.";
                string subject = "Welcome to Gardenwaala.com";
                SendEmail.EmailBody("Guest", nmodel.Email, emailMsg, subject);
                return Json(new { Status = true, Msg = "Thank you for subscribing us." });
            }
            else
            {
                return Json(new { Status = false, Msg = "Error in submitting your data! Please try again." });
            }
        }

        public ViewResult ContactUs()
        {
            TempData["activePg"] = "C";
            ViewBag.ActivePage = "contact";
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ContactForm()
        {
            return PartialView("_ContactForm");
        }

        [HttpPost]
        public JsonResult Contact(EnquiryViewModel model)
        {
            model.EnqDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                //string logoImg = repoBusi.GetLogo(UserDetails.Uid);
                IEnquiryRepository repoEnq = new EnquiryRepository();

                if (repoEnq.Add(model))
                {
                    ViewBag.Msg = "Thank you for contacting us! We will contact you soon.";
                    ViewBag.Status = true;

                    //Communication.SendEMail(model.Name, model.EmailId, model.ContactNo, model.Subject, model.Description);
                    return Json(new { status = true, msg = "Thank you for contacting us! We will contact you soon." });
                }
                else
                {

                    return Json(new { status = false, msg = "Error in submitting your data! Please try again." });

                }
            }
            else
            {
                return Json("ContactUs");
            }
        }


        public ViewResult Career()
        {
            ICareerRepository repoCarrer = new CareerRepository();
            IEnumerable<CareerViewModel> lstCarrer = repoCarrer.GetHomeJob();
            return View(lstCarrer);
        }
    }
}