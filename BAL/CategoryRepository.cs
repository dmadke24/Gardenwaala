using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;

namespace BAL
{
    public interface ICategoryRepository
    {
        bool Add(CategoryViewModel model);
        bool Update(CategoryViewModel model);
        bool Delete(int id);
        CategoryViewModel GetById(int id);//here this id is used for edit purpose
        CategoryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        IEnumerable<DropDownViewModel> GetCategoryList();
        IEnumerable<CategoryViewModel> GetCategory();
        //IEnumerable<CategoryAPIViewModel> GetAPIPCategory();
    }

    public class CategoryRepository : ICategoryRepository
    {
        MyDBContext db = new MyDBContext();

        private int NewId()
        {
            string oldid = Convert.ToString(db.Categories.Select(ft => ft.CategoryId).DefaultIfEmpty().Max());

            string myid, newid;
            int year = DateTime.Now.Year;
            if (oldid == "0")
            {
                myid = year + "0001";
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
                    newid = old_year + old_no.ToString("0000");
                }
                else
                    newid = year + "0001";

                return (Convert.ToInt32(newid));
            }

        }

        public bool Add(CategoryViewModel model)
        {
            try
            {
                Category obj = new Category();
                obj.CategoryId = NewId();
                obj.CategoryName = model.CategoryName;
                obj.Image = model.Image;
                obj.Status = model.Status;
                db.Categories.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(CategoryViewModel model)
        {
            try
            {
                Category obj = new Category();
                obj.CategoryId = model.CategoryId;
                obj.CategoryName = model.CategoryName;
                obj.Image = model.Image;
                obj.Status = model.Status;
                db.Entry<Category>(obj).State = System.Data.Entity.EntityState.Modified;
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
                Category obj = db.Categories.Find(id);
                if (obj != null)
                {
                    db.Categories.Remove(obj);
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

        public CategoryViewModel GetById(int id)
        {
            CategoryViewModel model = null;
            Category obj = db.Categories.Find(id);
            if (obj != null)
            {
                model = new CategoryViewModel();
                model.CategoryId = obj.CategoryId;
                model.CategoryName = obj.CategoryName;
                model.Image = obj.Image;
                model.Status = obj.Status;
                model.OldFile = obj.Image;
            }
            return model;
        }

        public CategoryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN Category I.E. OF CategoryGridViewModel'
            CategoryGridViewModel gridModel = new CategoryGridViewModel();

            IQueryable<Category> lstCategory = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstCategory = db.Categories;
            }
            else
            {
                lstCategory = db.Categories.Where(ft => ft.CategoryName.ToLower().Contains(searchStr));

            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "Categoryname":
                    lstCategory = sortDir == "asc" ? lstCategory.OrderBy(ft => ft.CategoryName) : lstCategory.OrderByDescending(ft => ft.CategoryName);
                    break;

                default:
                    lstCategory = lstCategory.OrderBy(ft => ft.CategoryId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstCategory.Count();

            //pagination code start******************************************************
            lstCategory = lstCategory.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<CategoryViewModel> lstModel = new List<CategoryViewModel>();

            foreach (Category obj in lstCategory)
            {
                CategoryViewModel model = new CategoryViewModel();
                model.CategoryId = obj.CategoryId;
                model.CategoryName = obj.CategoryName;
                model.Image = obj.Image;
                model.Status = obj.Status;
                lstModel.Add(model);
            }

            gridModel.CategoryList = lstModel;
            return gridModel;
        }


        public IEnumerable<DropDownViewModel> GetCategoryList()
        {
            IEnumerable<DropDownViewModel> lstModel = db.Database.SqlQuery<DropDownViewModel>("Select CategoryId As ID, CategoryName As Text From Category");
            return lstModel;
        }


        public IEnumerable<CategoryViewModel> GetCategory()
        {
            IEnumerable<CategoryViewModel> lstModel = db.Database.SqlQuery<CategoryViewModel>("Select * From Category where Status=1 order by CategoryId");
            return lstModel;
        }

    }
}
