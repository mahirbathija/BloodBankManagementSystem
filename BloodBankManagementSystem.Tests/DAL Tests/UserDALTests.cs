using System;
using System.Data;
using System.Linq;
using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;

namespace BloodBankManagementSystem.Tests.DAL_Tests;

public class UserDALTests
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
    public void userDAL_Select_CanSelectAllUsersCorrectly()
    {
        var mockDatabase = new MockDatabase();
        
        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);
        
        // mocking string sql = "SELECT * FROM tbl_users";
        mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
        {
            var usersSelected = mockDatabase.SelectUsersIntoDataSet(dataSet);
            return usersSelected;
        });

        var dataTable = userDAL.Select();
        
        Assert.NotNull(dataTable);

        var usersSelected = dataTable.Rows;
        
        Assert.NotNull(usersSelected);
        Assert.Equals(mockDatabase.mockUsers.Count, usersSelected.Count);
    }

    [Test]
    public void userDAL_Insert_CanInsertUserCorrectly()
    {
        var mockDatabase = new MockDatabase();
        
        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

        var newUser = new userBLL
        {
            user_id = 2,
            username = "mahirbathija",
            email = "mahirbathija@gmail.com",
            password = "abc123",
            full_name = "Mahir Bathija",
            contact = "2066930757",
            address = "1000 Minor Ave",
            added_date = DateTime.Now,
            image_name = "mahir_profile_picture"
        };

        mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
        {
            var rowsInserted = mockDatabase.InsertUser(newUser);
            return rowsInserted;
        });

        userDAL.Insert(newUser);

        Assert.AreEqual(2, mockDatabase.mockUsers.Count);
        Assert.True(mockDatabase.mockUsers.Any(u => u.user_id == newUser.user_id));
    }
}
