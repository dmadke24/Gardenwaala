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
    public class StateRepository : IStateRepository
    {
        MyDBContext db = new MyDBContext();

        public bool Add(StateViewModel model)
        {
            State obj = new State();
            try
            {
                obj.SName = model.SName;
                obj.StateId = model.StateId;
                obj.CountryId = model.CountryId;
                db.States.Add(obj);

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(StateViewModel model)
        {
            try
            {
                State obj = new State();
                obj.SName = model.SName;
                obj.StateId = model.StateId;
                obj.CountryId = model.CountryId;

                db.Entry<State>(obj).State = EntityState.Modified;
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
                State obj = db.States.Find(id);
                if (obj != null)
                {
                    db.States.Remove(obj);
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

        public StateViewModel GetById(int id)
        {
            try
            {
                StateViewModel model = new StateViewModel();

                State obj = db.States.Find(id);
                if (obj != null)
                {
                    model.StateId = obj.StateId;
                    model.SName = obj.SName;
                    model.CountryId = obj.CountryId;
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

        public StateGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF StateGridViewModel'
            StateGridViewModel gridModel = new StateGridViewModel();

            IQueryable<State> lstState = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstState = db.States;
            }
            else
            {
                lstState = db.States.Where(s => s.SName.ToLower().Contains(searchStr) || s.Country.CName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "cname":
                    lstState = sortDir == "asc" ? lstState.OrderBy(s => s.Country.CName) : lstState.OrderByDescending(s => s.Country.CName);
                    break;

                case "sname":
                    lstState = sortDir == "asc" ? lstState.OrderBy(s => s.SName) : lstState.OrderByDescending(s => s.SName);
                    break;

                default:
                    lstState = lstState.OrderBy(s => s.StateId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstState.Count();

            //pagination code start******************************************************
            lstState = lstState.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstState to StateList in gridModel i.e. StateGridViewModel
            List<StateViewModel> lstModel = new List<StateViewModel>();

            foreach (State obj in lstState.ToList<State>())
            {
                StateViewModel model = new StateViewModel();

                model.StateId = obj.StateId;
                model.SName = obj.SName;
                model.CountryId = obj.CountryId;
                model.CName = obj.Country.CName;

                lstModel.Add(model);
            }

            gridModel.StateList = lstModel;
            return gridModel;
        }


        public IEnumerable<DropDownViewModel> GetStates()
        {
            //raw SQL query
            IEnumerable<DropDownViewModel> lstState = db.Database.SqlQuery<DropDownViewModel>("Select StateId as ID, SName as Text From States");
            return lstState;
        }
    }
}
