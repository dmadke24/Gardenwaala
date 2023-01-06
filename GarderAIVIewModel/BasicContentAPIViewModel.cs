using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenAPIViewModel
{
    public class BannerAPIViewModel
    {
        public bool status { get; set; }

        public string ImageURL { get; set; }
        public string msg { get; set; }

        public IEnumerable<BannerViewModel> data { get; set; }
    }

    public class AboutUsAPIViewMOdel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public IEnumerable<BannerViewModel> data { get; set; }
    }

    public class FAQAPIViewMOdel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public IEnumerable<FAQViewModel> data { get; set; }
    }

    public class DiscountCouponAPIViewMOdel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public IEnumerable<DiscountCouponViewModel> data { get; set; }
    }

    public class ValidateCouponAPIViewMOdel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public DiscountCouponPercetgViewModel data { get; set; }
    }



}
