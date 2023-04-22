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
        private Mock<IDbConnection> mockDbConnection;
        private Mock<IDbDataAdapter> mockDbDataAdapter;
        private Mock<IDbCommand> mockDbCommand;
        private Mock<IDbDataParameter> mockParameter;
        private Mock<IDataParameterCollection> mockDataParameterCollection;

        [SetUp]
        public void Setup()
        {
            mockDbConnection = new Mock<IDbConnection>();
            mockDbDataAdapter = new Mock<IDbDataAdapter>();
            mockDbCommand = new Mock<IDbCommand>();
            mockParameter = new Mock<IDbDataParameter>();
            mockDataParameterCollection = new Mock<IDataParameterCollection>();
            
            mockDbConnection.SetupAllProperties();
            mockDbDataAdapter.SetupAllProperties();
            mockDbCommand.SetupAllProperties();
            mockParameter.SetupAllProperties();
            mockDataParameterCollection.SetupAllProperties();

            mockDbCommand.Setup(m => m.CreateParameter()).Returns(mockParameter.Object);
            mockDbCommand.SetupGet(m => m.Parameters).Returns(mockDataParameterCollection.Object);
            
            mockDbConnection.Setup(m => m.CreateCommand()).Returns(mockDbCommand.Object);
        }
        
        [Test]
        public void loginDal_loginCheck_Success()
        {
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