using ShipDashboardApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShipDashboardApp.Data
{
    public class ShipdashboardService : IShipdashboardsService
    {
        private string shipdashboardFile = "shipdashboards.json"; private IList<Shipdashboard> shipdashboards; public ShipdashboardService()
        {
            if (!File.Exists(shipdashboardFile))
            {
                Seed();
                WriteShipdashboardsToFile();
            }
            else
            {
                string content = File.ReadAllText(shipdashboardFile);
                shipdashboards = JsonSerializer.Deserialize<List<Shipdashboard>>(content);
            }
        }

        public IList<Shipdashboard> GetShipdashboards() { 
            List<Shipdashboard> tmp = new List<Shipdashboard>(shipdashboards); 
            return tmp; 
        }
        public void AddShipdashboard(Shipdashboard shipdashboard) {
            int max;
            if (shipdashboards.Any())
            {
                max = shipdashboards.Max(shipdashboard => shipdashboard.ShipdashboardId); 
            }
            else
            {
                max = 0;
            }

            shipdashboard.ShipdashboardId = (++max); shipdashboards.Add(shipdashboard);
            shipdashboard.DateAndTime = DateTime.Now;
            WriteShipdashboardsToFile();
        }
        public void RemoveShipdashboard(int shipdashboardId)
        {
            Shipdashboard toRemove = shipdashboards.First(t => t.ShipdashboardId == shipdashboardId);
            shipdashboards.Remove(toRemove);
            WriteShipdashboardsToFile();
        }
        public void Update(Shipdashboard shipdashboard) { 
            Shipdashboard toUpdate = shipdashboards.First(t => t.ShipdashboardId == shipdashboard.ShipdashboardId); 
            toUpdate.IsCompleted = shipdashboard.IsCompleted;
            WriteShipdashboardsToFile(); 
        }

        private void WriteShipdashboardsToFile()
        {
            string productsAsJson = JsonSerializer.Serialize(shipdashboards);
            File.WriteAllText(shipdashboardFile, productsAsJson);
        }

        private void Seed() { 
            Shipdashboard[] ts = { 
                new Shipdashboard { UserName = "OlesiewiA", ShipdashboardId = 1, Title = "v", IsCompleted = false }, 
                new Shipdashboard { UserName = "OlesiewiA", ShipdashboardId = 2, Title = "v", IsCompleted = false },
                new Shipdashboard { UserName = "OlesiewiA", ShipdashboardId = 3, Title = "v", IsCompleted = true }, 
                new Shipdashboard { UserName = "OlesiewiA", ShipdashboardId = 4, Title = "v", IsCompleted = false }, 
                new Shipdashboard { UserName = "OlesiewiA", ShipdashboardId = 5, Title = "v", IsCompleted = true }, 
            };
            shipdashboards = ts.ToList(); 
        }
    }
}
