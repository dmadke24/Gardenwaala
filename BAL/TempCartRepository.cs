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
    public class TempCartRepository : ITempCartRepository
    {
        MyDBContext db = new MyDBContext();

        private int NewId()
        {
            int oldid = db.TempCarts.Select(a => a.TempId).DefaultIfEmpty().Max();
            int myid, newid;
            if (oldid == 0)
            {
                myid = 1;
                return myid;
            }
            else
            {
                newid = oldid + 1;
                return (newid);
            }
        }

        public IEnumerable<TempCartViewModel> Save(TempCartViewModel model)
        {
            try
            {
                TempCart obj = db.TempCarts.Where(t => t.UserId == model.UserId && t.ProductId == model.ProductId).FirstOrDefault();
                IQueryable<TempCart> lstTCart = null;

                IProductRepository repoFmnu = new ProductRepository();
                if (obj == null)
                {
                    TempCart objAdd = new TempCart();
                    objAdd.TempId = NewId();
                    objAdd.UserId = model.UserId;
                    objAdd.ProductId = model.ProductId;
                    objAdd.Quantity = model.Quantity;
                    objAdd.Price = model.Price;

                    db.TempCarts.Add(objAdd);

                    db.SaveChanges();

                    //select data for updated cart
                    lstTCart = db.TempCarts.Where(t => t.UserId == model.UserId);
                    List<TempCartViewModel> lstModel = new List<TempCartViewModel>();

                    foreach (TempCart objCart in lstTCart)
                    {
                        Product fobj = db.Products.Find(objCart.ProductId);

                        TempCartViewModel Cmodel = new TempCartViewModel();
                        Cmodel.UserId = objCart.UserId;
                        Cmodel.ProductId = objCart.ProductId;
                        Cmodel.Quantity = objCart.Quantity;
                        Cmodel.Price = objCart.Price;
                        Cmodel.ProdName = fobj.Name;
                        Cmodel.TempId = objCart.TempId;

                        Cmodel.Image = fobj.Image1;

                        lstModel.Add(Cmodel);
                    }
                    return lstModel;
                }
                else
                {
                    obj.Quantity += model.Quantity;
                    db.SaveChanges();

                    //select data for updated cart
                    lstTCart = db.TempCarts.Where(t => t.UserId == model.UserId);
                    List<TempCartViewModel> lstModel = new List<TempCartViewModel>();

                    foreach (TempCart objCart in lstTCart)
                    {
                        Product fobj = db.Products.Find(objCart.ProductId);

                        TempCartViewModel Cmodel = new TempCartViewModel();
                        Cmodel.UserId = objCart.UserId;
                        Cmodel.ProductId = objCart.ProductId;
                        Cmodel.Quantity = objCart.Quantity;
                        Cmodel.Price = objCart.Price;
                        Cmodel.ProdName = fobj.Name;
                        Cmodel.TempId = objCart.TempId;
                        Cmodel.Image = fobj.Image1;

                        lstModel.Add(Cmodel);
                    }
                    return lstModel;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IEnumerable<TempCartViewModel> GetMyCart(int UserId)
        {
            IQueryable<TempCart> lstTCart = null;
            //select data for updated cart
            lstTCart = db.TempCarts.Where(t => t.UserId == UserId);
            List<TempCartViewModel> lstModel = new List<TempCartViewModel>();

            foreach (TempCart objCart in lstTCart.ToList())
            {
                Product fobj = db.Products.Find(objCart.ProductId);

                TempCartViewModel Cmodel = new TempCartViewModel();
                Cmodel.TempId = objCart.TempId;
                Cmodel.UserId = objCart.UserId;
                Cmodel.ProductId = objCart.ProductId;
                Cmodel.Quantity = objCart.Quantity;
                Cmodel.Price = objCart.Price;
                Cmodel.ProdName = fobj.Name;
                Cmodel.Image = fobj.Image1;

                lstModel.Add(Cmodel);

            }
            return lstModel;
        }


        public bool DeleteCartItem(int TempId, int UserId)
        {
            IQueryable<TempCart> lstTCart = null;
            //select data for updated cart
            TempCart obj = db.TempCarts.Where(t => t.TempId == TempId && t.UserId == UserId).FirstOrDefault();

            if (obj != null)
            {
                db.TempCarts.Remove(obj);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
