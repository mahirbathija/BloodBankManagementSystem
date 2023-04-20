using System;
using System.Data.SqlClient;
using BloodBankManagementSystem.BLL;
using Moq;
using NUnit.Framework;

namespace BloodBankManagementSystem.Tests
{
    public class LoginDALTests
    {
        [Test]
        public void loginBll_CanCreate_Get_Set()
        {
            var sqlConMock = new Mock<SqlConnection>();
            var mockCmd = new Mock<SqlCommand>();
            mockCmd.Object.Connection = sqlConMock.Object;
            mockCmd.Object.CommandText = "SELECT * FROM tbl_users WHERE username=@username AND password=@password";
            
            mockCmd.Object.Parameters.AddWithValue("@username", "mahri");
            mockCmd.Object.Parameters.AddWithValue("@password", "xyz");

            var x = 5;
        }
    }
}