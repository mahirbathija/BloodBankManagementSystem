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

    public int DeleteUser(int userId)
    {
        var matchingUser = mockUsers.First(u => u.user_id == userId);
        
        if (matchingUser == null)
            return 0;

        var removed = mockUsers.Remove(matchingUser);

        return removed ? 1 : 0;
    }

    public int UpdateUser(userBLL user)
    {
        var matchingUser = mockUsers.First(u => u.user_id == user.user_id);
        
        if (matchingUser == null)
            return 0;

        matchingUser.username = user.username;
        matchingUser.email = user.email;
        matchingUser.password = user.password;
        matchingUser.full_name = user.full_name;
        matchingUser.contact = user.contact;
        matchingUser.address = user.address;
        matchingUser.added_date = user.added_date;
        matchingUser.image_name = user.image_name;

        return 1;
    }

    public int SearchUserByKeyword(DataSet dataSet, string keyword)
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

        var matchingUsers = mockUsers.Where(u =>
        {
            var matchingId = u.user_id.ToString().Contains(keyword);
            var matchingFullName = u.full_name.Contains(keyword);
            var matchingAddress = u.address.Contains(keyword);

            return matchingId || matchingAddress || matchingFullName;
        }).ToList();

        foreach (var user in matchingUsers)
        {
            table.Rows.Add(user.user_id, user.username, user.email, user.password, user.full_name, user.contact, user.address, user.added_date, user.image_name);    
        }

        dataSet.Tables.Add(table);

        return matchingUsers.Count;
    }

    public int GetIdFromUsername(DataSet dataSet, string username)
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

        var matchingUser = mockUsers.First(u => u.username == username);

        if (matchingUser == null)
        {
            dataSet.Tables.Add(table);
            return 0;
        }

        table.Rows.Add(matchingUser.user_id, matchingUser.username, matchingUser.email, matchingUser.password, matchingUser.full_name, matchingUser.contact, matchingUser.address, matchingUser.added_date, matchingUser.image_name);

        dataSet.Tables.Add(table);

        return 1;
    }

    public int InsertUser(userBLL user)
    {
        mockUsers.Add(user);
        return 1;
    }

}