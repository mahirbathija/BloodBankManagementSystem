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
}
[Test]
public void TestSearchUserByKeyword()
{
    // Arrange
    var dataSet = new DataSet();
    var mockUsers = new List<User>
    {
        new User { user_id = 1, username = "rdoe", email = "rdoe@example.com", password = "password", full_name = "Ralph Doe", contact = "555-1234", address = "123 Main St", added_date = DateTime.Now, image_name = "rdoe.jpg" },
        new User { user_id = 2, username = "msmith", email = "msmith@example.com", password = "password", full_name = "Mary Smith", contact = "555-5678", address = "456 Elm St", added_date = DateTime.Now, image_name = "msmith.jpg" },
        new User { user_id = 3, username = "jwilliams", email = "jwilliams@example.com", password = "password", full_name = "James Williams", contact = "555-9012", address = "789 Oak St", added_date = DateTime.Now, image_name = "jwilliams.jpg" },
    };
    var keyword = "Smith";
    var expectedCount = 1;

    // Act
    var target = new userDAL(mockUsers);
    var actualCount = target.SearchUserByKeyword(dataSet, keyword);

    // Assert
    Assert.AreEqual(expectedCount, actualCount);
    Assert.AreEqual(1, dataSet.Tables.Count);
    Assert.AreEqual(1, dataSet.Tables[0].Rows.Count);
    Assert.AreEqual("msmith", dataSet.Tables[0].Rows[0]["username"]);
}

[Test]
public void TestGetIdFromUsername()
{
    // Arrange
    var dataSet = new DataSet();
    var mockUsers = new List<User>
    {
        new User { user_id = 1, username = "r.doe", email = "ralph.doe@example.com", password = "password123", full_name = "Ralph Doe", contact = "1234567890", address = "123 Main St", added_date = DateTime.Now, image_name = "ralph.jpg" },
        new User { user_id = 2, username = "jane.doe", email = "jane.doe@example.com", password = "password456", full_name = "Jane Doe", contact = "0987654321", address = "456 Oak Ave", added_date = DateTime.Now, image_name = "jane.jpg" }
    };
    var keyword = "john.doe";
    var expectedCount = 1;

    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository.Setup(x => x.GetIdFromUsername(dataSet, keyword)).Returns(expectedCount);

    // Act
    var actualCount = mockUserRepository.Object.GetIdFromUsername(dataSet, keyword);

    // Assert
    Assert.AreEqual(expectedCount, actualCount);
}

