using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;
using System.Data.Entity.Validation;

namespace BAL
{

    public interface IProductRepository
    {
        bool Add(ProductViewModel model);
        bool Update(ProductViewModel model);
        bool Delete(int id);
        ProductViewModel GetById(int id);//here this id is used for edit purpose
        ProductGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<ProductViewModel> GetHomeProduct(int pid, int subid);
        IEnumerable<ProductViewModel> GetHomeProductCat(int pid, int subid);


        IEnumerable<ProductViewModel> GetRelatedProduct(int pid);
        IEnumerable<ProductViewModel> GetHomeSlideProducts();
        IEnumerable<ProductViewModel> GetBestSeller(int cnt = 0);
        IEnumerable<ProductViewModel> GetSearchData(string SearchData);
        OrderViewModel GetProductDetail(int id);//here this id is used for edit purpose
        IEnumerable<ProductViewModel> GetProductBySubCat(int scid);
        ProductViewModel GetDetailProduct(int id);

    }

    public class ProductRepository : IProductRepository
    {
        MyDBContext db = new MyDBContext();


        private int NewId()
        {
            string oldid = Convert.ToString(db.Products.Select(fm => fm.ProductId).DefaultIfEmpty().Max());

            string myid, newid;
            int year = DateTime.Now.Year;
            if (oldid == "0")
            {
                myid = year + "00001";
                return Convert.ToInt32(myid);
            }
            else
            {

                //Split the id into two parts year and no
                string old_year = oldid.Substring(0, 4);
                int old_no = Convert.ToInt32(oldid.Substring(4));

                if (year == Convert.ToInt32(old_year))
                {
                    old_no += 1;
                    newid = old_year + old_no.ToString("00000");
                }
                else
                    newid = year + "00001";

                return (Convert.ToInt32(newid));
            }

        }


        public bool Add(ProductViewModel model)
        {
            try
            {
                Product obj = new Product();
                obj.ProductId = NewId();
                obj.Name = model.Name;
                obj.Description = model.Description;
                obj.Price = model.Price;
                obj.CategoryId = model.CategoryId;
                obj.SubCategoryId = model.SubCategoryId;
                obj.Status = model.Status;
                obj.Priority = (byte)model.Priority;
                obj.Image1 = model.Image1;
                obj.Image2 = model.Image2;
                obj.Image3 = model.Image3;
                obj.NoOfPieces = model.NoOfPieces;
                obj.NetWeight = model.NetWeight;
                obj.GrossWeight = model.GrossWeight;
                obj.Height = model.Height;
                obj.Size = model.Size;
                obj.HomeSlide = model.HomeSlide;
                obj.BestSeller = model.BestSeller;
                obj.OfferPrice = model.OfferPrice;
                obj.Stock = model.Stock;
                db.Products.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
        }

        public bool Update(ProductViewModel model)
        {
            try
            {
                Product obj = new Product();
                obj.ProductId = model.ProductId;
                obj.Name = model.Name;
                obj.Description = model.Description;
                obj.Price = model.Price;
                obj.CategoryId = model.CategoryId;
                obj.SubCategoryId = model.SubCategoryId;
                obj.Status = model.Status;
                obj.Priority = (byte)model.Priority;
                obj.Image1 = model.Image1;
                obj.Image2 = model.Image2;
                obj.Image3 = model.Image3;
                db.Entry<Product>(obj).State = System.Data.Entity.EntityState.Modified;
                obj.NoOfPieces = model.NoOfPieces;
                obj.NetWeight = model.NetWeight;
                obj.Height = model.Height;
                obj.Size = model.Size;
                obj.GrossWeight = model.GrossWeight;
                obj.HomeSlide = model.HomeSlide;
                obj.BestSeller = model.BestSeller;
                obj.OfferPrice = model.OfferPrice;
                obj.Stock = model.Stock;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Product obj = db.Products.Find(id);
                if (obj != null)
                {
                    db.Products.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public ProductViewModel GetById(int id)
        {
            ProductViewModel model = null;
            Product obj = db.Products.Find(id);
            if (obj != null)
            {
                model = new ProductViewModel();

                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                model.Description = obj.Description;
                model.Price = obj.Price;
                model.CategoryId = obj.CategoryId;
                model.SubCategoryId = obj.SubCategoryId;

                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.OldImage1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.OldImage2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.OldImage3 = obj.Image3;
                model.NoOfPieces = obj.NoOfPieces;
                model.GrossWeight = obj.GrossWeight;
                model.NetWeight = obj.NetWeight;
                model.Height = obj.Height;
                model.Size = obj.Size;
                if (obj.OfferPrice != null)
                    model.OfferPrice = obj.OfferPrice;
                else
                    model.OfferPrice = 0;
                model.HomeSlide = obj.HomeSlide;
                model.BestSeller = obj.BestSeller;
                model.Stock = obj.Stock;

            }
            return model;
        }

        public ProductGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            ProductGridViewModel gridModel = new ProductGridViewModel();

            IQueryable<Product> lstProduct = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstProduct = db.Products.OrderBy(p => p.Priority);
            }
            else
            {
                lstProduct = db.Products.Where(fm => (fm.Name.ToLower().Contains(searchStr) || fm.Category.CategoryName.ToLower().Contains(searchStr))).OrderBy(p => p.Priority);
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "name":
                    lstProduct = sortDir == "asc" ? lstProduct.OrderBy(fm => fm.Name) : lstProduct.OrderByDescending(fm => fm.Name);
                    break;

                case "CategoryName":
                    lstProduct = sortDir == "asc" ? lstProduct.OrderBy(fm => fm.Category.CategoryName) : lstProduct.OrderByDescending(fm => fm.Category.CategoryName);
                    break;

                default:
                    lstProduct = lstProduct.OrderBy(fm => fm.ProductId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstProduct.Count();

            //pagination code start******************************************************
            lstProduct = lstProduct.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<ProductViewModel> lstModel = new List<ProductViewModel>();

            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();

                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description.Substring(0, obj.Description.Length <= 100 ? obj.Description.Length : 100) + " ...";
                model.Price = obj.Price;
                model.CategoryId = obj.CategoryId;
                model.CategoryName = obj.Category.CategoryName;
                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.HomeSlide = obj.HomeSlide;
                model.BestSeller = obj.BestSeller;
                model.Stock = obj.Stock;

                lstModel.Add(model);
            }

            gridModel.ProductList = lstModel;
            return gridModel;
        }

        public void ChangeStatus(IEnumerable<int> Ids, bool status)
        {
            foreach (int id in Ids)
            {
                Banner c = db.Banners.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }

        //listing on first page
        public IEnumerable<ProductViewModel> GetHomeProduct(int pid, int subid)
        {
            int ProductSubCategoryId = 0;
            //if (subid == 0)
            //{
            //    ProductSubCategoryId = db.Products.Where(fm => fm.Status == true && fm.CategoryId == pid).Select(fm => fm.SubCategoryId).FirstOrDefault();
            //}
            //else
            //{
            //    ProductSubCategoryId = subid;
            //}
            IQueryable<Product> lstProduct = null;

            lstProduct = db.Products.Where(fm => fm.Status == true && fm.SubCategoryId == pid);

            List<ProductViewModel> lstModel = new List<ProductViewModel>();
            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();
                model.ProductId = obj.ProductId;
                model.CategoryName = obj.Category.CategoryName;
                model.Name = obj.Name;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;
                model.Price = obj.Price;
                model.OfferPrice = obj.OfferPrice;
                model.CategoryId = obj.CategoryId;
                model.SubCategoryName = obj.SubCategory.SubCategoryName;
                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.NoOfPieces = obj.NoOfPieces;
                model.NetWeight = obj.NetWeight;
                model.GrossWeight = obj.GrossWeight;
                model.Height = obj.Height;
                model.Size = obj.Size;
                lstModel.Add(model);
            }
            return lstModel;
        }

        public IEnumerable<ProductViewModel> GetRelatedProduct(int pid)
        {
            //Get the product category by product id
            int categoryId = db.Products.Where(t => t.ProductId == pid).Select(t => t.CategoryId).FirstOrDefault();

            IQueryable<Product> lstProduct = null;

            lstProduct = db.Products.Where(fm => fm.Status == true && fm.ProductId != pid && fm.CategoryId == categoryId);

            List<ProductViewModel> lstModel = new List<ProductViewModel>();
            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();
                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;
                model.Price = obj.Price;
                model.OfferPrice = obj.OfferPrice;
                model.CategoryId = obj.CategoryId;
                model.CategoryName = obj.Category.CategoryName;
                model.SubCategoryName = obj.SubCategory.SubCategoryName;
                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.NoOfPieces = obj.NoOfPieces;
                model.NetWeight = obj.NetWeight;
                model.GrossWeight = obj.GrossWeight;
                model.Height = obj.Height;
                model.Size = obj.Size;
                lstModel.Add(model);
            }
            return lstModel;
        }

        public IEnumerable<ProductViewModel> GetHomeSlideProducts()
        {
            IQueryable<Product> lstProduct = null;

            lstProduct = db.Products.Where(fm => fm.Status == true && fm.HomeSlide == true);

            List<ProductViewModel> lstModel = new List<ProductViewModel>();
            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();
                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                model.Image1 = obj.Image1;
                model.HomeSlide = obj.HomeSlide;
                model.Price = obj.Price;
                model.OfferPrice = obj.OfferPrice;
                lstModel.Add(model);
            }
            return lstModel;
        }


        public IEnumerable<ProductViewModel> GetBestSeller(int cnt=0)
        {
            IQueryable<Product> lstProduct = null;
            if (cnt != 0)
            {
                lstProduct = db.Products.Where(fm => fm.Status == true && fm.BestSeller == true).Take(cnt);
            }
            else
                lstProduct = db.Products.Where(fm => fm.Status == true && fm.BestSeller == true);

            List<ProductViewModel> lstModel = new List<ProductViewModel>();
            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();
                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                model.Image1 = obj.Image1;
                model.Price = obj.Price;
                model.OfferPrice = obj.OfferPrice;
                lstModel.Add(model);
            }
            return lstModel;
        }


        public IEnumerable<ProductViewModel> GetSearchData(string srarchdata)
        {
            IEnumerable<ProductViewModel> lstModel = db.Database.SqlQuery<ProductViewModel>(string.Format("Select * from Products where Status=1 and Name like '%{0}%'", srarchdata));
            return lstModel;
        }

        public OrderViewModel GetProductDetail(int id)
        {
            OrderViewModel model = null;
            //Product obj = db.Products.Find(id);
            //string[] cutTypesList = new string[10];
            //if (obj != null)
            //{
            //    model = new OrderViewModel();

            //    model.ProductId = obj.ProductId;
            //    model.Name = obj.Name;
            //    model.Description = obj.Description;
            //    model.Price = obj.Price;
            //    model.ProductTypeId = obj.ProductTypeId;
            //    model.ProductSubCategoryId = obj.ProductSubCategoryId;
            //    model.Status = obj.Status;
            //    model.Image1 = obj.Image1;
            //    model.NoOfPieces = obj.NoOfPieces;
            //    model.GrossWeight = obj.GrossWeight;
            //    model.NetWeight = obj.NetWeight;
            //    model.HomeSlide = obj.HomeSlide;
            //    model.BestSeller = obj.BestSeller;
            //    model.Height = obj.Height;
            //    model.Size = obj.Size;
            //    if (obj.CutTypes != null)
            //    {
            //        cutTypesList = obj.CutTypes.Split(',');
            //        model.CutTypeList = cutTypesList;
            //    }
            //    else
            //    {
            //        cutTypesList[0] = null;
            //        model.CutTypeList = cutTypesList;
            //    }

           // }
            return model;
        }

        public IEnumerable<ProductViewModel> GetProductBySubCat(int scid)
        {
            IEnumerable<ProductViewModel> lstModel = db.Database.SqlQuery<ProductViewModel>(string.Format("Select Top 8 * From Products where SubCategoryId = {0} order by ProductId", scid));
            return lstModel;
        }

        public IEnumerable<ProductViewModel> GetHomeProductCat(int pid, int subid)
        {
            IQueryable<Product> lstProduct = null;

            lstProduct = db.Products.Where(fm => fm.Status == true && fm.CategoryId == pid);

            List<ProductViewModel> lstModel = new List<ProductViewModel>();
            foreach (Product obj in lstProduct.ToList<Product>())
            {
                ProductViewModel model = new ProductViewModel();
                model.ProductId = obj.ProductId;
                model.CategoryName = obj.Category.CategoryName;
                model.Name = obj.Name;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;
                model.Price = obj.Price;
                model.OfferPrice = obj.OfferPrice;
                model.CategoryId = obj.CategoryId;
                model.SubCategoryName = obj.SubCategory.SubCategoryName;
                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.NoOfPieces = obj.NoOfPieces;
                model.NetWeight = obj.NetWeight;
                model.GrossWeight = obj.GrossWeight;
                model.Height = obj.Height;
                model.Size = obj.Size;
                lstModel.Add(model);
            }
            return lstModel;
        }

        public ProductViewModel GetDetailProduct(int id)
        {
            ProductViewModel model = null;
            Product obj = db.Products.Find(id);
            if (obj != null)
            {
                model = new ProductViewModel();

                model.ProductId = obj.ProductId;
                model.Name = obj.Name;
                model.Description = obj.Description;
                model.Price = obj.Price;
                model.CategoryId = obj.CategoryId;
                model.SubCategoryId = obj.SubCategoryId;

                model.Status = obj.Status;
                model.Priority = obj.Priority;
                model.Image1 = obj.Image1;
                model.OldImage1 = obj.Image1;
                model.Image2 = obj.Image2;
                model.OldImage2 = obj.Image2;
                model.Image3 = obj.Image3;
                model.OldImage3 = obj.Image3;
                model.NoOfPieces = obj.NoOfPieces;
                model.GrossWeight = obj.GrossWeight;
                model.NetWeight = obj.NetWeight;
                model.Height = obj.Height;
                model.Size = obj.Size;
                if (obj.OfferPrice != null)
                    model.OfferPrice = obj.OfferPrice;
                else
                    model.OfferPrice = 0;
                model.HomeSlide = obj.HomeSlide;
                model.BestSeller = obj.BestSeller;
                model.Stock = obj.Stock;

            }
            return model;
        }
    }
}
