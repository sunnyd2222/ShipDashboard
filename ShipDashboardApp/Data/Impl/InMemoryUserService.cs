using System;
using System.Collections.Generic;
using System.Linq;
using ShipDashboardApp.Models;

namespace ShipDashboardApp.Data.Impl
{
    public class InMemoryUserService : IUserService
    {

        User user;

        private List<User> users;
        public InMemoryUserService() {
            //Roles:
            //"User" = Can controll own ShipDashboards 
            //"Admin" = Can controll all ShipDashboards

            //SecurityLevel:
            //1 = Can view ShipDashboards determined by Role
            //2 = Cam view, create and delete ShipDashboards determined by Role

            users = new[] {
                new User {
                    City = "Mtown",
                    Domain = "emea",
                    Password = "dffg^#gju(*yghn^rrgfy%",
                    Role = "view",
                    BirthYear = 0000,
                    SecurityLevel = 1,
                    ID = 2,
                    UserName = "Viewer"
                },
                new User {
                    City = "Mtown",
                    Domain = "emea",
                    Password = "df$f%y^jhgj&*hrg@#$gfhgj*&)jhgfbdsf#",
                    Role = "Admin",
                    BirthYear = 0000,
                    ID = 3,
                    SecurityLevel = 2,
                    UserName = "Admin"
                }
            }.ToList();
        }


        public User ValidateUser(string userName, string password) {
            User first = users.FirstOrDefault(user => user.UserName.Equals(userName));
            if (first == null) {
                throw new Exception("User not found");
            }

            if (!first.Password.Equals(password)) {
                throw new Exception("Incorrect password");
            }
            user = first;
            return first;
        }
    }
}