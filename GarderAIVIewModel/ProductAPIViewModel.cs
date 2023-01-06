using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenAPIViewModel
{

    public class CategoryAPIViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }
        public string ImageURL { get; set; }

        public IEnumerable<CategoryViewModel> data { get; set; }
    }

    public class SubCategoryAPIViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }


        public IEnumerable<SubCategoryViewModel> data { get; set; }
    }

    public class ProductListAPIViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public string ImageURL { get; set; }

        public IEnumerable<ProductViewModel> data { get; set; }
    }

    public class ProductDetailAPIViewModel
    {
        public bool status { get; set; }

        public string ImageURL { get; set; }
        public string msg { get; set; }

        public ProductViewModel data { get; set; }
    }

    public class OrderListAPIViewModel
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public OrderViewModel data { get; set; }
    }



}
