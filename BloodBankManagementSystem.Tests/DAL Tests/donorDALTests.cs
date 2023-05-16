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
        public void donorDAL_Select_Success()
        {
            var mockDatabase = new MockDatabase();

            var DonorDAL = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            mockDbDataAdapter.Setup(m => m.Fill(It.IsAny<DataSet>())).Returns((DataSet dataSet) =>
            {
                var donorsSelected = mockDatabase.SelectDonors(dataSet);
                return donorsSelected;
            });

            var dataTable = DonorDAL.Select();

            Assert.NotNull(dataTable);

            var donorsSelected = dataTable.Rows;

            Assert.NotNull(donorsSelected);
            Assert.AreEqual(mockDatabase.mockDonors.Count, donorsSelected.Count);
        }


        [Test]
        public void donorDAL_Insert_Success()
        {
            var mockDatabase = new MockDatabase();

            var donorCount = mockDatabase.mockDonors.Count;

            var donorDAL = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            var donor = new donorBLL
            {

                donor_id = 3,
                first_name = "sean",
                last_name = "li",
                email = "test@seattleu.edu",
                contact = "test",
                gender = "male",
                address = "isekai Dr.",
                blood_group = "O+",
                added_date = DateTime.Now,
                image_name = "cubes",
                added_by = 1


            };

            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsInserted = mockDatabase.AddDonor(donor);
                return rowsInserted;
            });

            var added = donorDAL.Insert(donor);

            Assert.True(added);
            Assert.AreEqual(donorCount + 1, mockDatabase.mockDonors.Count);
            Assert.True(mockDatabase.mockDonors.Any(u => u.donor_id == donor.donor_id));
        }

        [Test]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = false)]
        public bool donorDAL_Update_Success(int donorId)
        {
            var mockDatabase = new MockDatabase();

            var donorDAL = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            var updatedDonor = new donorBLL { donor_id = donorId };

            var updatedDateTime = DateTime.Now;

            var updateDonorValues = new donorBLL
            {
                donor_id = updatedDonor.donor_id,
                first_name = "new_fname",
                last_name = "new_lname",
                email = "new_email",
                contact = "new_contact",
                gender = "new_gender",
                address = "new_address",
                blood_group = "A+",
                added_date = updatedDateTime,
                image_name = "new_image",
                added_by = 1,
            };

            mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(() =>
            {
                var rowsUpdated = mockDatabase.UpdateDonor(updateDonorValues);
                return rowsUpdated;
            });

            var updated = donorDAL.Update(updatedDonor);

            var updatedMockDonor = mockDatabase.mockDonors.FirstOrDefault(d => d.donor_id == d.donor_id);

            if (updatedMockDonor == null)
            {
                Assert.False(mockDatabase.mockDonors.Any(d => d.donor_id == donorId));
                return updated;
            }

            Assert.True(mockDatabase.mockDonors.Any(d => d.donor_id == updatedMockDonor.donor_id));
            Assert.AreEqual(updateDonorValues.first_name, updatedMockDonor.first_name);
            Assert.AreEqual(updateDonorValues.last_name, updatedMockDonor.last_name);
            Assert.AreEqual(updateDonorValues.email, updatedMockDonor.email);
            Assert.AreEqual(updateDonorValues.contact, updatedMockDonor.contact);
            Assert.AreEqual(updateDonorValues.gender, updatedMockDonor.gender);
            Assert.AreEqual(updateDonorValues.address, updatedMockDonor.address);
            Assert.AreEqual(updateDonorValues.blood_group, updatedMockDonor.blood_group);
            Assert.IsTrue((updatedMockDonor.added_date - updateDonorValues.added_date).Duration() <= TimeSpan.FromSeconds(1));
            Assert.AreEqual(updateDonorValues.image_name, updatedMockDonor.image_name);
            Assert.AreEqual(updateDonorValues.added_by, updatedMockDonor.added_by);

            return updated;
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
        [TestCase("A-", ExpectedResult = 1, TestName = "Search_Returns_value_exist(AB+)")]
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


