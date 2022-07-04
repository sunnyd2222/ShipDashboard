using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipDashboardApp.Data
{
    public interface IShipdashboardsService
    {
        IList<Models.Shipdashboard> GetShipdashboards(); 
        void AddShipdashboard(Models.Shipdashboard shipdashboard); 
        void RemoveShipdashboard(int shipdashboardId);
        void Update(Models.Shipdashboard shipdashboard);
    }
}
