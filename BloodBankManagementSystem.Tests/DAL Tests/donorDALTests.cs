using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        [TestCase("", ExpectedResult =4, TestName = "Search_ReturnscorrectValue_Donor_Id(1)")]
        public int SearchDonor_Id(String Keyword)
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


