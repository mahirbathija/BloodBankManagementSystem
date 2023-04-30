using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Castle.Core.Configuration;
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
        [TestCase("Nidhan", "123", ExpectedResult = true, TestName = "LoginDAL_check_true_Available_inTable(Nidhan,123)")]
        [TestCase("Nidhan", "", ExpectedResult = true, TestName = "LoginDAL_check_true_Available_inTable(Nidhan,)")]
        [TestCase("Nidhan", null, ExpectedResult = true, TestName = "LoginDAL_check_true_Available_inTable(Nidhan,null)")]
        public bool loginDal_loginCheck_Success(string Login_Username, string Login_Password)
        {
            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                // Create mock data
                var table = new DataTable("MockUserTable");
                table.Columns.Add("username", typeof(string));
                table.Columns.Add("password", typeof(string));
                table.Rows.Add(Login_Username, Login_Password);

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

            return success;
        }



        [Test]
        [TestCase("Nidhan", "123", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(Nidhan,123)")]
        [TestCase("", "", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(,)")]
        [TestCase(null, null, ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(null,null)")]
        [TestCase("Nidhan", "", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(Nidhan,)")]
        [TestCase("", "123", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(,123)")]
        [TestCase("Nidhan", null, ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(Nidhan,null)")]
        [TestCase(null, "123", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_Available_inTable(null,123)")]
        public Boolean loginDal_loginCheck_fail(String Login_Username, String Login_Password)
        {
            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                // Create mock data
                var table = new DataTable("MockUserTable");
                table.Columns.Add("username", typeof(string));
                table.Columns.Add("password", typeof(string));
                // Add the mock data to the dataset
                dataSet.Tables.Add(table);


                return table.Rows.Count;
            });

            var loginBll = new loginBLL
            {
                username = Login_Username,
                password = Login_Password,
            };

            var loginDal = new loginDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            var failure = loginDal.loginCheck(loginBll);

            return failure;
        }


        [Test]
        [TestCase("Sean", "pass", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(Sean,pass)")]
        [TestCase("admin", "pass", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(admin,pass)")]
        [TestCase("Sean", "admin", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(Sean,pass)")]
        [TestCase("", "pass", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(Sean,pass)")]
        [TestCase("Sean", "", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(Sean,)")]
        [TestCase("", "", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(,)")]
        [TestCase(null, null, ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(null,null)")]
        [TestCase(null, "", ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(null,)")]
        [TestCase("Sean", null, ExpectedResult = false, TestName = "LoginDAL_check_false_Not_inFilledTable(Sean,null)")]

        public Boolean loginDal_loginCheck_withData_Fail(String Login_Username, String Login_Password)
        {
            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                // Create mock data
                var table = new DataTable("MockUserTable");
                table.Columns.Add("username", typeof(string));
                table.Columns.Add("password", typeof(string));

                // Add the mock data to the dataset
                dataSet.Tables.Add(table);

                return table.Rows.Count;
            });

            var loginBll = new loginBLL
            {
                username = Login_Username,
                password = Login_Password,
            };

            var loginDal = new loginDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            var loginResult = loginDal.loginCheck(loginBll);

            return loginResult;
        }

    }
}