using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;

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

             new ()
        {
            donor_id = 1,
            first_name = "iol",
            last_name = "qwe",
            email = "iol@yahoo.com",
            contact = "45673421",
            gender = "Male",
            address = "Minor Ave",
            blood_group ="O+",
            added_date = DateTime.Now,
            image_name = "io_profile_picture",
            added_by = 2
        },
             new ()
        {
            donor_id = 2,
            first_name = "pol",
            last_name = "uio",
            email = "poluio@hotmail.com",
            contact = "89012345",
            gender = "female",
            address = "asdf",
            blood_group ="O+",
            added_date = DateTime.Now,
            image_name = "pol_profile_picture",
            added_by = 2
        },
      new()
     {
         donor_id = 3,
         first_name = "iop",
         last_name = "tyu",
         email = "iop@seattleu.edu",
         contact = "6789102345",
         gender = "Male",
         address = "qacvb",
         blood_group = "O-",
         added_date = DateTime.Now,
         image_name = "iop_profile_picture",
         added_by = 1
     },
             new()
         {
             donor_id = 4,
             first_name = "aui",
             last_name = "fgh",
             email = "fgh@gmail.com",
             contact = "19876456",
             gender = "Female",
             address = "ghjk",
             blood_group = "O-",
             added_date = DateTime.Now,
             image_name = "aui_profile_picture",
             added_by = 2
         },
             new()
        {
            donor_id = 5,
            first_name = "Mahir",
            last_name = "Bathija",
            email = "mahirbathija@gmail.com",
            contact = "2066930757",
            gender = "Male",
            address = "1000 Minor Ave",
            blood_group ="B+",
            added_date = DateTime.Now,
            image_name = "mahir_profile_picture",
            added_by = 1
        },
             new()
        {
            donor_id = 6,
            first_name = "abc",
            last_name = "def",
            email = "abcdef@gmail.com",
            contact = "12345678",
            gender = "Male",
            address = "ghi",
            blood_group ="B+",
            added_date = DateTime.Now,
            image_name = "abc_profile_picture",
            added_by = 1
        },
             new()
     {
         donor_id = 7,
         first_name = "Jame",
         last_name = "Robinson",
         email = "jame@gmail.com",
         contact = "91011121314",
         gender = "Male",
         address = "jkl",
         blood_group = "B-",
         added_date = DateTime.Now,
         image_name = "jame_profile_picture",
         added_by = 1
     },
             new()
         {
             donor_id = 8,
             first_name = "jai",
             last_name = "rob",
             email = "jai@gmail.com",
             contact = "15161718",
             gender = "Male",
             address = "mno",
             blood_group = "B-",
             added_date = DateTime.Now,
             image_name = "jai_profile_picture",
             added_by = 1
         },
             new()
         {
             donor_id = 9,
             first_name = "yfg",
             last_name = "ier",
             email = "yfg@gmail.com",
             contact = "48247901",
             gender = "Male",
             address = "pqwer",
             blood_group = "A-",
             added_date = DateTime.Now,
             image_name = "yfg_profile_picture",
             added_by = 3
         }

    };

    public MockDatabase()
    {
        this.mockDonors = new List<donorBLL>(initialMockDonors);
        this.mockUsers = new List<userBLL>(initialMockUsers);
    }

    public void ResetMockUsers()
    {
        this.mockUsers = initialMockUsers;
    }

    public void ResetMockDonors()
    {
        this.mockDonors = initialMockDonors;
    }
    #region userDAL
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
    #endregion
    public List<userDAL> SearchUserByKeyword(string keyword)
    {
        var results = new List<userDAL>();

        foreach (var user in mockUsers)
        {
            if (user.username.Contains(keyword) || user.full_name.Contains(keyword) || user.email.Contains(keyword))
            {
                results.Add(user);

            }
           
        }
        return results;
    }

    #region donorDAL

    public int DeleteDonor(int DonorId)
    {
        try
        {
            var matchingDonor = mockDonors.First(d => d.donor_id == DonorId);
            if (matchingDonor == null)
                return 0;

            var removed = mockDonors.Remove(matchingDonor);

            return removed ? 1 : 0;
        }
        catch (Exception exception)
        {
            return 0;
        }

    }

    //Find Donors by Blood Group
    public int FindDonorsByBloodGroup(DataSet dataSet, string BloodGroup)
    {
        var table = new DataTable("MockDonorTable");
        table.Columns.Add("donor_id", typeof(int));
        table.Columns.Add("first_name", typeof(string));
        table.Columns.Add("last_name", typeof(string));
        table.Columns.Add("email", typeof(string));
        table.Columns.Add("contact", typeof(string));
        table.Columns.Add("gender", typeof(string));
        table.Columns.Add("address", typeof(string));
        table.Columns.Add("blood_group", typeof(string));
        table.Columns.Add("added_date", typeof(DateTime));
        table.Columns.Add("image_name", typeof(string));
        table.Columns.Add("added_by", typeof(int));


        var matchingDonor = mockDonors.Where(d => d.blood_group == BloodGroup);

        if (matchingDonor == null)
        {
            dataSet.Tables.Add(table);
            return 0;
        }
        foreach (var donor in matchingDonor)
        {
            table.Rows.Add(donor.donor_id, donor.first_name, donor.last_name, donor.email, donor.contact, donor.gender, donor.address, donor.blood_group, donor.added_date, donor.image_name, donor.added_by);

        }
        dataSet.Tables.Add(table);
        return matchingDonor.Count();
    }

    //Find Donors By keyword
    public int FindDonorsByKeyword(DataSet dataSet, string keyword)
    {
        var table = new DataTable("MockDonorTable");
        table.Columns.Add("donor_id", typeof(int));
        table.Columns.Add("first_name", typeof(string));
        table.Columns.Add("last_name", typeof(string));
        table.Columns.Add("email", typeof(string));
        table.Columns.Add("contact", typeof(string));
        table.Columns.Add("gender", typeof(string));
        table.Columns.Add("address", typeof(string));
        table.Columns.Add("blood_group", typeof(string));
        table.Columns.Add("added_date", typeof(DateTime));
        table.Columns.Add("image_name", typeof(string));
        table.Columns.Add("added_by", typeof(int));


        var matchingDonor = mockDonors.Where(d =>
        {
            var matchingDonorId = d.donor_id.ToString().Contains(keyword);
            var matchingFirstName = d.first_name.Contains(keyword);
            var matchingLastName = d.last_name.Contains(keyword);
            var matchingEmail = d.email.Contains(keyword);
            var matchingBloodGroup = d.blood_group.Contains(keyword);
            return matchingDonorId || matchingFirstName || matchingLastName || matchingEmail || matchingBloodGroup;

        }).ToList();


        foreach (var donor in matchingDonor)
        {
            table.Rows.Add(donor.donor_id, donor.first_name, donor.last_name, donor.email, donor.contact, donor.gender, donor.address, donor.blood_group, donor.added_date, donor.image_name, donor.added_by);
        }
        dataSet.Tables.Add(table);
        return matchingDonor.Count;
    }

    internal object SearchUsersInTheDataSet(DataSet dataSet)
    {
        throw new NotImplementedException();
    }

    internal static object SearchUsersByKeyword(string v)
    {
        throw new NotImplementedException();
    }

    internal static int GetIDFromUsername(string username)
    {
        throw new NotImplementedException();
    }
}


    #endregion
    