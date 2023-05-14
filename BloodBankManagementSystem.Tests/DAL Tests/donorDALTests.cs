using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Tests.DAL_Tests
{
    public class donorDALTests
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
        public void Select_Returns_All_Donors()
        {
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

            // Act
            var dataTable = Donordal.Select();

            // Assert
            Assert.NotNull(dataTable);
        }





        [Test]
        [TestCase(1, "ftest", "ltest", "email", "contact", "male", "temp" ,"O+", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(allNormal)")]
        [TestCase(2, "", "test", "email", "contact", "female", "temp", "O-", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_fname)")]
        [TestCase(3, "test", "", "email", "contact", "male", "temp", "A+", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_lname)")]
        [TestCase(4, "test", "test", "", "contact", "female", "temp", "A-", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_email)")]
        [TestCase(5, "test", "test", "email", "", "male", "temp", "B+", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_contact)")]
        [TestCase(6, "test", "test", "email", "contact", "", "temp", "B-", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_gender)")]
        [TestCase(7, "test", "test", "email", "contact", "male", "", "AB+", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_address)")]
        [TestCase(8, "test", "test", "email", "contact", "male", "temp", "", ExpectedResult = true, TestName = "DonorDal_insert_true_Available_inTable(no_Btype)")]

        public bool DonorDAL_Insert_Success(int Donor_id, String fname, String lname, String email, String contact, 
            String sex, String address, String bloodType)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

            donorBLL donors = new donorBLL
            {
                donor_id = Donor_id,
                first_name = fname,
                last_name = lname,
                email = email,
                contact = contact,
                gender = sex,
                address = address,
                blood_group = bloodType,
                added_date = DateTime.Now,
                image_name = "test",
                added_by = 1
            };

            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsInserted = mockDatabase.AddDonor(donors);
                return rowsInserted;
            });

            // Act
            var result = Donordal.Insert(donors);
            // Assert
            return result;
        }

       
        [Test]
        [TestCase(1, "ftest", "ltest", "email", "contact", "male", "temp", "O+", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(allNormal)")]
        [TestCase(2, "", "test", "email", "contact", "female", "temp", "O-", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_fname)")]
        [TestCase(3, "test", "", "email", "contact", "male", "temp", "A+", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_lname)")]
        [TestCase(4, "test", "test", "", "contact", "female", "temp", "A-", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_email)")]
        [TestCase(5, "test", "test", "email", "", "male", "temp", "B+", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_contact)")]
        [TestCase(6, "test", "test", "email", "contact", "", "temp", "B-", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_gender)")]
        [TestCase(7, "test", "test", "email", "contact", "male", "", "AB+", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_address)")]
        [TestCase(8, "test", "test", "email", "contact", "male", "temp", "", ExpectedResult = true, TestName = "DonorDal_update_true_Available_inTable(no_Btype)")]

        public bool DonorDAL_Update_Success(int Donor_id, String fname, String lname, String email, String contact,
            String sex, String address, String bloodType)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var donorDal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

            //Create donor object to update
            var donorToUpdate = new donorBLL
            {
                donor_id = Donor_id,
                first_name = fname,
                last_name = lname,
                email = email,
                contact = contact,
                gender = sex,
                address = address,
                blood_group = bloodType,
                added_date = DateTime.Now,
                image_name = "test",
                added_by = 1
            };

            //Setup mock command to return number of rows affected by update query
            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsUpdated = mockDatabase.UpdateDonor(donorToUpdate);
                return rowsUpdated;
            });

            //Act
            var result = donorDal.Update(donorToUpdate);

            //Assert
            return result;
        }


        [Test]
        [TestCase(1, ExpectedResult = true, TestName = "DonorDAL_check_true_Available_inTable(1)")]
        [TestCase(2, ExpectedResult = true, TestName = "DonorDAL_check_true_Available_inTable(2)")]
        [TestCase(3, ExpectedResult = true, TestName = "DonorDAL_check_true_Available_inTable(3)")]
        [TestCase(4, ExpectedResult = true, TestName = "DonorDAL_check_true_Available_inTable(4)")]
        [TestCase(5, ExpectedResult = true, TestName = "DonorDAL_check_true_Available_inTable(5)")]
        public Boolean DonorDALDelete_Success(int Donor_id)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

           

            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsInserted = mockDatabase.DeleteDonor(Donor_id);
                return rowsInserted;
            });

            var donor = new donorBLL { donor_id = Donor_id };

            // Act
            var result = Donordal.Delete(donor);
            // Assert
            return result;
        }


        [Test]
        [TestCase(-1, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(-1)")]
        [TestCase(0, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(0)")]
        [TestCase(-2, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(-2)")]
        [TestCase(10, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(10)")]
        [TestCase(100, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(100)")]
        [TestCase(11, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(11)")]

        public Boolean DonorDALDelete_Failure(int Donor_id)
        {

            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);


            donorBLL donors = new donorBLL
            {

                donor_id = Donor_id,
                first_name = "test",
                last_name = "test",
                email = "test",
                contact = "test",
                gender = "male",
                address = "test",
                blood_group = "O+",
                added_date = DateTime.Now,
                image_name = "test",
                added_by = 1


            };


            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsInserted = mockDatabase.DeleteDonor(donors.donor_id);
                return rowsInserted;
            });

            var donor = new donorBLL { donor_id = Donor_id };

            // Act
            var result = Donordal.Delete(donor);
            // Assert
            return result;

        }




        [Test]
        [TestCase("A-", ExpectedResult = "1", TestName = "CountDonors_ReturnsCorrectValue(A-)")]
        [TestCase("B+", ExpectedResult = "2", TestName = "CountDonors_ReturnsCorrectValue(B+)")]
        [TestCase("B-", ExpectedResult = "2", TestName = "CountDonors_ReturnsCorrectValue(B-)")]
        [TestCase("O+", ExpectedResult = "2", TestName = "CountDonors_ReturnsCorrectValue(O+)")]  
        [TestCase("O-", ExpectedResult = "2", TestName = "CountDonors_ReturnsCorrectValue(O-)")]
        public String CountDonors_ReturnsCorrectValue(String bloodGroup)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand

            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                var rowsCount = mockDatabase.FindDonorsByBloodGroup(dataSet, bloodGroup);
                return rowsCount;
            });

            // Act
            var result = Donordal.countDonors(bloodGroup);
            return result;

        }

        [Test]
        [TestCase("A+", ExpectedResult = "0", TestName = "CountDonors_ReturnsZeroValue(A+)")]
        [TestCase("AB+", ExpectedResult = "0", TestName = "CountDonors_ReturnsZeroValue(AB+)")]
        [TestCase("AB-", ExpectedResult = "0", TestName = "CountDonors_ReturnsZeroValue(AB-)")]
        public String CountDonors_ReturnsZerotValue(String bloodGroup)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();
            //Donor dal
            var Donordal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Setup the mock IDbConnection to return the mock IDbCommand

            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                var rowsCount = mockDatabase.FindDonorsByBloodGroup(dataSet, bloodGroup);
                return rowsCount;
            });

            // Act
            var result = Donordal.countDonors(bloodGroup);
            return result;

        }



        [Test]
        //Donor Id
        [TestCase("6", ExpectedResult =1, TestName = "Search_Returns_value_exist(6)")]
        [TestCase("1",ExpectedResult =1,TestName ="Search_Returns_value_exist(1)")]
        [TestCase("-1", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(-1)")]
        [TestCase("0", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(0)")]
        [TestCase("100", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(100)")]

        //First Name
        [TestCase("yfg", ExpectedResult = 1, TestName = "Search_Returns_value_exist(yfg)")]
        [TestCase("nidhan", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(nidhan)")]

        //Last Name
        [TestCase("ier", ExpectedResult = 1, TestName = "Search_Returns_value_exist(ier)")]
        [TestCase("xyz", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(xyz)")]

        //Email
        [TestCase("yfg@gmail.com", ExpectedResult = 1, TestName = "Search_Returns_value_exist(yfg@gmail.com)")]
        [TestCase("nidhanbhatt@gmail.com", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(nidhanbhatt@gmail.com")]
       
        //  BloodGroup
        [TestCase("B+", ExpectedResult = 2, TestName = "Search_Returns_value_exist(B+)")] 
        [TestCase("B-", ExpectedResult = 2, TestName = "Search_Returns_value_exist(B-)")]
        [TestCase("AB+", ExpectedResult = 1, TestName = "Search_Returns_value_exist(AB+)")]
        [TestCase("F+", ExpectedResult = 0, TestName = "Search_Returns_value_not_exist(F+)")]

        public int SearchDonor(String Keyword)
        {
            //Mock Database
            var mockDatabase = new MockDatabase();

            var donor = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            
            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                var donorSelected= mockDatabase.FindDonorsByKeyword(dataSet, Keyword);
                return donorSelected;
            });

            var dataTable = donor.Search(Keyword);

            Assert.NotNull(dataTable);

            var donorsSelected = dataTable.Rows;
            return donorsSelected.Count;
        }
    }
}


