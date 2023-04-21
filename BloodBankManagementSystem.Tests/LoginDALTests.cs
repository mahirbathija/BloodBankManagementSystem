using System.Collections.Generic;
using System.Data;
using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;

namespace BloodBankManagementSystem.Tests
{
    public class LoginDALTests
    {
        [Test]
        public void loginDal_loginCheck_Success()
        {
            var mockDbConnection = new Mock<IDbConnection>();
            var mockDbDataAdapter = new Mock<IDbDataAdapter>();
            var mockDbCommand = new Mock<IDbCommand>();
            var mockParameter = new Mock<IDbDataParameter>();
            var mockDataParameterCollection = new Mock<IDataParameterCollection>();
            
            mockDbConnection.SetupAllProperties();
            mockDbDataAdapter.SetupAllProperties();
            mockDbCommand.SetupAllProperties();
            mockParameter.SetupAllProperties();
            mockDataParameterCollection.SetupAllProperties();

            mockDbCommand.Setup(m => m.CreateParameter()).Returns(mockParameter.Object);
            mockDbCommand.SetupGet(m => m.Parameters).Returns(mockDataParameterCollection.Object);

            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                // Create mock data
                var table = new DataTable("MockUserTable");
                table.Columns.Add("username", typeof(string));
                table.Columns.Add("password", typeof(string));
                table.Rows.Add("admin", "admin");

                // Add the mock data to the dataset
                dataSet.Tables.Add(table);

                return table.Rows.Count;
            });        
            
            mockDbConnection.Setup(m => m.CreateCommand()).Returns(mockDbCommand.Object);

            var loginBll = new loginBLL
            {
                username = "admin",
                password = "admin",
            };
            
            var loginDal = new loginDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            var success = loginDal.loginCheck(loginBll);
            
            Assert.True(success);
        }
    }
}