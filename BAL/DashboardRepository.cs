using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenViewModel;
using System.Data.SqlClient;
using DAL;

namespace BAL
{

    public interface IDashboardRepository
    {
        IEnumerable<VisitorCountViewModel> GetVisitorCount();
    }

    public class DashboardRepository : IDashboardRepository
    {
        MyDBContext db = new MyDBContext();

        public IEnumerable<VisitorCountViewModel> GetVisitorCount()
        {
            IEnumerable<VisitorCountViewModel> model = db.Database.SqlQuery<VisitorCountViewModel>("SELECT Top(12) DATEPART(YEAR, VisitDt) AS VisitorYear,DATEPART(MONTH, VisitDt) AS VisitorMonth,Left(DateName(month,VisitDt),3) As Mon, COUNT(*) AS VisitorCnt FROM Visitors GROUP BY DATEPART(YEAR, VisitDt),DATEPART(MONTH,VisitDt),Left(DateName(month,VisitDt),3) order by VisitorYear DESC, VisitorMonth DESC");
            return model;
        }

    }
}
