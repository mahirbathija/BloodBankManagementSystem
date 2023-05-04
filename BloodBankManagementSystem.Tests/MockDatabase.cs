using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BloodBankManagementSystem.BLL;

namespace BloodBankManagementSystem.Tests;

public class MockDatabase
{

    public IList<userBLL> mockUsers;
    public IList<donorBLL> mockDonors;

    public static IList<userBLL> initialMockUsers = new List<userBLL>
    {
        new()
        {
            user_id = 1,
            username = "mahirbathija",
            email = "mahirbathija@gmail.com",
            password = "abc123",
            full_name = "Mahir Bathija",
            contact = "2066930757",
            address = "1000 Minor Ave",
            added_date = DateTime.Now,
            image_name = "mahir_profile_picture"
        }
    };

    public static IList<donorBLL> initialMockDonors = new List<donorBLL>
    {
        new()
        {
            donor_id = 1,
            first_name = "Mahir",
            last_name = "Bathija",
            email = "mahirbathija@gmail.com",
            contact = "2066930757",
            gender = "Male",
            address = "1000 Minor Ave",
            blood_group = "B Positive",
            added_date = DateTime.Now,
            image_name = "mahir_profile_picture",
            added_by = 1  
        }
    };

    public MockDatabase()
    {
        this.mockDonors = initialMockDonors;
        this.mockUsers = initialMockUsers;
    }

    public void ResetMockUsers()
    {
        this.mockUsers = initialMockUsers;
    }
    
    public void ResetMockDonors()
    {
        this.mockDonors = initialMockDonors;
    }

    public int FindUsersWithLoginCredentials(string username, string password)
    {
        return mockUsers.Count(u => u.username == username && u.password == password);
    }
    
    public int SelectUsersIntoDataSet(DataSet dataSet)
    {
        var table = new DataTable("MockUserTable");
        table.Columns.Add("user_id", typeof(int));
        table.Columns.Add("username", typeof(string));
        table.Columns.Add("email", typeof(string));
        table.Columns.Add("password", typeof(string));
        table.Columns.Add("full_name", typeof(string));
        table.Columns.Add("contact", typeof(string));
        table.Columns.Add("address", typeof(string));
        table.Columns.Add("added_data", typeof(DateTime));
        table.Columns.Add("image_name", typeof(string));

        foreach (var user in mockUsers)
        {
            table.Rows.Add(user.user_id, user.username, user.email, user.password, user.full_name, user.contact, user.address, user.added_date, user.image_name);    
        }

        dataSet.Tables.Add(table);

        return mockUsers.Count;
    }

}