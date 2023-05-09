using System;
using System.Data;
using System.Data.SqlClient;
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
        Assert.AreEqual(mockDatabase.mockUsers.Count, usersSelected.Count);
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

        var inserted = userDAL.Insert(newUser);

        Assert.True(inserted);
        Assert.AreEqual(2, mockDatabase.mockUsers.Count);
        Assert.True(mockDatabase.mockUsers.Any(u => u.user_id == newUser.user_id));
    }
    
    
    [Test]
    public void userDAL_Delete_CanDeleteUserCorrectly()
    {
        var mockDatabase = new MockDatabase();
        
        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

        var userToDelete = mockDatabase.mockUsers.First();

        mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
        {
            var rowsDeleted = mockDatabase.DeleteUser(userToDelete.user_id);
            return rowsDeleted;
        });

        var deleted = userDAL.Delete(userToDelete);
        
        Assert.True(deleted);
        Assert.AreEqual(0, mockDatabase.mockUsers.Count);
        Assert.False(mockDatabase.mockUsers.Any(u => u.user_id == userToDelete.user_id));
    }
    
    [Test]
    public void userDAL_Update_CanUpdateUserCorrectly()
    {
        var mockDatabase = new MockDatabase();
        
        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

        var userToUpdate = mockDatabase.mockUsers.First();

        var updatedDateTime = DateTime.Now;

        var userWithUpdatedValues = new userBLL
        {
            user_id = userToUpdate.user_id,
            username = "updated_username",
            email = "updated_email" ,
            password = "updated_password" ,
            full_name = "updated_full_name" ,
            contact = "updated_contact" ,
            address = "updated_address" ,
            added_date = updatedDateTime,
            image_name = "updated_image_name" 
        };

        mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
        {
            var rowsUpdated = mockDatabase.UpdateUser(userWithUpdatedValues);
            return rowsUpdated;
        });

        var updated = userDAL.Update(userToUpdate);

        var updatedMockUser = mockDatabase.mockUsers.First(u => u.user_id == userToUpdate.user_id);
        
        Assert.True(updated);
        Assert.AreEqual(1, mockDatabase.mockUsers.Count);
        Assert.True(mockDatabase.mockUsers.Any(u => u.user_id == updatedMockUser.user_id));
        Assert.AreEqual(userWithUpdatedValues.username, updatedMockUser.username);
        Assert.AreEqual(userWithUpdatedValues.email, updatedMockUser.email);
        Assert.AreEqual(userWithUpdatedValues.password, updatedMockUser.password);
        Assert.AreEqual(userWithUpdatedValues.full_name, updatedMockUser.full_name);
        Assert.AreEqual(userWithUpdatedValues.contact, updatedMockUser.contact);
        Assert.AreEqual(userWithUpdatedValues.address, updatedMockUser.address);
        Assert.AreEqual(userWithUpdatedValues.added_date, updatedMockUser.added_date);
        Assert.AreEqual(userWithUpdatedValues.image_name, updatedMockUser.image_name); 
    }

  
    [Test]
    public void TestingSearchMethod()
    {
        var mockDatabase = new MockDatabase();
        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

        // mocking string sql = "SELECT * FROM tbl_users";
        mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
        {
            var usersSelected = mockDatabase.SearchUsersInTheDataSet(dataSet);
            return usersSelected;
        });
       
       


    // Act
    var results = userDAL.Search("Stephen"); 

    // Assert
    Assert.AreEqual(2, results.Count); 
    Assert.AreEqual("StephenDoe", results[0].username);
    Assert.AreEqual("StephenDoe@example.com", results[0].email);
    Assert.AreEqual("Stephen Doe", results[0].full_name);
    Assert.AreEqual("StephenSmith", results[1].username);
    Assert.AreEqual("stephensmith@example.com", results[1].email);
    Assert.AreEqual("Stephen Smith", results[1].full_name);
}

[Test]
    public int getIDFromUsername()
    {
        var mockDatabase = new MockDatabase();

        var userDAL = new userDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

        // mocking string sql = "SELECT * FROM tbl_users";
        mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
        {
            var usersSelected = mockDatabase.SearchUsersInTheDataSet(dataSet);
            return usersSelected;
        });
       
       

        // Arrange
        string username = "stephen"; 
        var dataSet = new DataSet(); 
            var table = new DataTable();
    table.Columns.Add("user_id", typeof(int));


    int userID = MockDatabase.GetIDFromUsername(username);

    if (userID != -1)
    {
        // If the user ID is found, add a new row to the table with the user ID
        var row = table.NewRow();
        row["user_id"] = userID;
        table.Rows.Add(row);
    }

    // Add the table to the dataset and return the number of rows in the table
    dataSet.Tables.Add(table);
    return table.Rows.Count;
 
    }

        // Act
        int userID = userDAL.Search(username);

    public static object username { get; private set; }

   
  



}


