using System;
using System.Data;
using System.Data.SqlClient;
using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;

namespace BloodBankManagementSystem.Tests
{
    public class LoginDALTests
    {
        [Test]
        public void loginBll_CanCreate_Get_Set()
        {
            var sqlConMock = new Mock<IDbConnection>();
            var mockCmd = new Mock<IDbDataAdapter>();

            sqlConMock.Setup(x => x.CreateCommand()).CallBase();

            var loginBll = new loginBLL
            {
                username = "admin",
                password = "admin",
            };

            var ob1 = sqlConMock.Object;
            var ob2 = mockCmd.Object;
            loginDAL loginDal = new loginDAL();

            var success = loginDal.loginCheck(loginBll);
            
            Assert.True(success);
        }
    }
}