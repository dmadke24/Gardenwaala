using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;


namespace BAL
{
    public interface ITestimonialRepository
    {
        bool Add(TestimonialViewModel model);
        bool Update(TestimonialViewModel model);
        bool Delete(int id);
        TestimonialViewModel GetById(int id);
        TestimonialGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        string GetFileName(int id);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<TestimonialViewModel> GetHomeTestimonial();
    }

    public interface ICountryRepository
    {
        bool Add(CountryViewModel model);
        bool Update(CountryViewModel model);
        bool Delete(int id);
        CountryViewModel GetById(int id);
        CountryGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        IEnumerable<DropDownViewModel> GetCountries();

    }

    public interface IStateRepository
    {
        bool Add(StateViewModel model);
        bool Update(StateViewModel model);
        bool Delete(int id);
        StateViewModel GetById(int id);
        StateGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        IEnumerable<DropDownViewModel> GetStates();
    }

    public interface ICityRepository
    {
        bool Add(CityViewModel model);
        bool Update(CityViewModel model);
        bool Delete(int id);
        CityViewModel GetById(int id);
        CityGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<DropDownViewModel> GetCities();
    }

    public interface IAreaRepository
    {
        bool Add(AreaViewModel model);
        bool Update(AreaViewModel model);
        bool Delete(int id);
        AreaViewModel GetById(int id);
        AreaGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<AreaViewModel> GetAreas(int cid);
        bool DeliveryArea(string pincode);


    }

    public interface ITempCartRepository
    {
        IEnumerable<TempCartViewModel> Save(TempCartViewModel model);
        IEnumerable<TempCartViewModel> GetMyCart(int UserId);
        bool DeleteCartItem(int TempId, int UserId);
    }

    public interface ICareerRepository
    {
        bool Add(CareerViewModel model);
        bool Update(CareerViewModel model);
        bool Delete(int id);
        CareerGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        CareerViewModel GetById(int id);
        void ChangeStatus(IEnumerable<int> Ids, bool status);
        IEnumerable<CareerViewModel> GetHomeJob();

    }


}
