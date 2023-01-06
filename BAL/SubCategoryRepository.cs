using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using GardenViewModel;

namespace BAL
{

    public interface ISubCategoryRepository
    {
        bool Add(SubCategoryViewModel model);
        bool Update(SubCategoryViewModel model);
        bool Delete(int id);
        SubCategoryViewModel GetById(int id);
        SubCategoryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        IEnumerable<DropDownViewModel> GetSubCategory();
        IEnumerable<SubCategoryViewModel> GetSubCategoryMenu(int CatId);
        IEnumerable<SubCategoriesAPIViewModel> GetSubCategory(int SubCategoryId);

    }
    public class SubCategoryRepository : ISubCategoryRepository
    {
        MyDBContext db = new MyDBContext();

        private int NewId()
        {
            string oldid = Convert.ToString(db.SubCategories.Select(ft => ft.SubCategoryId).DefaultIfEmpty().Max());

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

        public bool Add(SubCategoryViewModel model)
        {
            try
            {
                SubCategory obj = new SubCategory();
                obj.SubCategoryId = NewId();
                obj.SubCategoryName = model.SubCategoryName;
                obj.CategoryId = model.CategoryId;
                if (model.Image == null || model.Image == "")
                    obj.Image = null;
                else
                    obj.Image = model.Image;

                db.SubCategories.Add(obj);

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(SubCategoryViewModel model)
        {
            try
            {
                SubCategory obj = new SubCategory();
                obj.SubCategoryId = model.SubCategoryId;
                obj.SubCategoryName = model.SubCategoryName;
                obj.CategoryId = model.CategoryId;
                if (model.Image == null || model.Image == "")
                    obj.Image = null;
                else
                    obj.Image = model.Image;

                db.Entry<SubCategory>(obj).State = EntityState.Modified;
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
                SubCategory obj = db.SubCategories.Find(id);
                if (obj != null)
                {
                    db.SubCategories.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public SubCategoryViewModel GetById(int id)
        {
            try
            {
                SubCategoryViewModel model = new SubCategoryViewModel();

                SubCategory obj = db.SubCategories.Find(id);
                if (obj != null)
                {
                    model.SubCategoryId = obj.SubCategoryId;
                    model.CategoryId = obj.CategoryId;
                    model.SubCategoryName = obj.SubCategoryName;
                    model.Image = obj.Image;

                    return model;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public SubCategoryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN Category I.E. OF SubCategoryGridViewModel'
            SubCategoryGridViewModel gridModel = new SubCategoryGridViewModel();

            IQueryable<SubCategory> lstSubCategory = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstSubCategory = db.SubCategories;
            }
            else
            {
                lstSubCategory = db.SubCategories.Where(c => c.SubCategoryName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "Categoryname":
                    lstSubCategory = sortDir == "asc" ? lstSubCategory.OrderBy(c => c.Category.CategoryName) : lstSubCategory.OrderByDescending(s => s.Category.CategoryName);
                    break;

                case "SubCategoryname":
                    lstSubCategory = sortDir == "asc" ? lstSubCategory.OrderBy(c => c.SubCategoryName) : lstSubCategory.OrderByDescending(c => c.SubCategoryName);
                    break;

                default:
                    lstSubCategory = lstSubCategory.OrderBy(c => c.SubCategoryId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstSubCategory.Count();

            //pagination code start******************************************************
            lstSubCategory = lstSubCategory.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstSubCategory to SubCategoryList in gridModel i.e. SubCategoryGridViewModel
            List<SubCategoryViewModel> lstModel = new List<SubCategoryViewModel>();

            foreach (SubCategory obj in lstSubCategory.ToList<SubCategory>())
            {
                SubCategoryViewModel model = new SubCategoryViewModel();

                model.SubCategoryId = obj.SubCategoryId;
                model.SubCategoryId = obj.SubCategoryId;
                model.SubCategoryName = obj.SubCategoryName;
                model.CategoryName = obj.Category.CategoryName;
                model.Image = obj.Image;

                lstModel.Add(model);
            }

            gridModel.SubCategoryList = lstModel;
            return gridModel;
        }

        public IEnumerable<DropDownViewModel> GetSubCategory()
        {
            IEnumerable<DropDownViewModel> lstSubCategory =
                db.Database.SqlQuery<DropDownViewModel>("Select SubCategoryId as ID, SubCategoryName as Text From SubCategory");
            return lstSubCategory;
        }

        public IEnumerable<SubCategoriesAPIViewModel> GetSubCategory(int SubCategoryId)
        {
            IEnumerable<SubCategoriesAPIViewModel> lstModel = db.Database.SqlQuery<SubCategoriesAPIViewModel>("Select fs.SubCategoryId, fs.SubCategoryName, fs.SubCategoryId, ft.Image From SubCategory fs JOIN Categorys ft on ft.SubCategoryId = fs.SubCategoryId where fs.SubCategoryId = " + SubCategoryId + " order by fs.SubCategoryId");
            return lstModel;
        }

        public IEnumerable<SubCategoryViewModel> GetSubCategoryMenu(int CatId)
        {
            IEnumerable<SubCategoryViewModel> lstModel = db.Database.SqlQuery<SubCategoryViewModel>(string.Format("Select sc.SubCategoryId,sc.SubCategoryName From SubCategory sc RIGHT JOIN Products p ON sc.SubCategoryId = p.SubCategoryId where sc.CategoryId = {0}  GROUP BY sc.SubCategoryId, sc.SubCategoryName", CatId));
            return lstModel;
        }
    }
}
