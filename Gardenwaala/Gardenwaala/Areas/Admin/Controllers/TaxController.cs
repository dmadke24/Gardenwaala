using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using GardenViewModel;
using BAL;

namespace Gardenwaala.Areas.Admin.Controllers.DiscountTax
{
    public class TaxController : UserAdminBaseController
    {
        ITaxRepository repoTax;
        public TaxController()
        {
            repoTax = new TaxRepository();
        }

        public ActionResult Index()
        {
            TaxViewModel model = repoTax.GetTaxDet();
            return View(model);
        }

        public RedirectToRouteResult Update(TaxViewModel model)
        {
            if (repoTax.Add(model))
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "Error in Saving Record, Try again!");
                return RedirectToAction("Index");

            }
        }
    }
}