using SV21T1080067.DataLayers;
using SV21T1080067.DataLayers.SQLServer;
using SV21T1080067.DomainModels;

namespace SV21T1080067.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;

        static UserAccountService()
        {
            employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAccountDB.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }

        }
    }
