using ShipDashboardApp.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShipDashboardApp.Models
{
    public class Shipdashboard
    {
        public string UserName{ get; set; }
        public int ShipdashboardId { get; set; }
        public string ShipmentType { get; set; }
        [Required, MaxLength(1024)] public string Title { get; set; }
        public DateTime DateAndTime { get; set; }
        [Required] public string ShipId { get; set; }
        public bool IsCompleted { get; set; }
        public Shipdashboard()
        {
        }
        public Shipdashboard(string user)
        {
            UserName = user;
        }
    }
}
