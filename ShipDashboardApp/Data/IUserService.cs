using ShipDashboardApp.Models;

namespace ShipDashboardApp.Data
{
    public interface IUserService
    {
        User ValidateUser(string userName, string Password);
    }
}