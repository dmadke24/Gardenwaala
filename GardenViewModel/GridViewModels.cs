using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{


    public class UserGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<UserViewModel> UserList { get; set; }

    }

    public class BannerGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<BannerViewModel> BannerList { get; set; }

    }


    public class TestimonialGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<TestimonialViewModel> TestimonialList { get; set; }
    }


    public class EnquiryGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<EnquiryViewModel> EnquiryList { get; set; }
    }

    public class WebRecentUpdateGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<RecentUpdateViewModel> RecentUpdateList { get; set; }
    }

    public class DiscountCouponGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<DiscountCouponViewModel> CouponList { get; set; }
    }

    public class CategoryGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<CategoryViewModel> CategoryList { get; set; }

    }


    public class SubCategoryGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<SubCategoryViewModel> SubCategoryList { get; set; }

    }

    public class ProductGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<ProductViewModel> ProductList { get; set; }

    }

    public class CountryGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<CountryViewModel> CountryList { get; set; }
    }

    public class StateGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<StateViewModel> StateList { get; set; }
    }

    public class CityGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<CityViewModel> CityList { get; set; }
    }

    public class AreaGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<AreaViewModel> AreaList { get; set; }
    }

    public class CareerGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<CareerViewModel> CareerList { get; set; }
    }
}


