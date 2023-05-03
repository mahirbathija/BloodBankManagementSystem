using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
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
        public Boolean DonorDALDelete_Success(int Donor_id)
        {

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

            // Setup the mock IDbCommand to return the mock IDataParameterCollection
            mockDbCommand.Setup(x => x.Parameters).Returns(mockDataParameterCollection.Object);

            IList<donorBLL> donors = new List<donorBLL>
                {
                    new()
                    {
                       donor_id=Donor_id,
                       first_name="test",
                       last_name="test",
                       email="test",
                       contact="test",
                       gender="male",
                       address="test",
                       blood_group="O+",
                       added_date=DateTime.Now,
                       image_name="test",
                       added_by=1

                    }
                };

            if (donors.Any(d => d.donor_id == Donor_id))
            {

                // Setup the mock IDbCommand to return a value of 1 when ExecuteNonQuery is called
                mockDbCommand.Setup(x => x.ExecuteNonQuery()).Returns(1);
            }


            var donor = new donorBLL { donor_id = Donor_id };

            var dal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);
            // Act
            var result = dal.Delete(donor);
            // Assert
            return result;
        }


        [Test]
        [TestCase(2, ExpectedResult = false, TestName = "DonorDAL_check_false_NotAvailable_inTable(2)")]
        public Boolean DonorDALDelete_Failure(int Donor_id)
        {

            // Setup the mock IDbConnection to return the mock IDbCommand
            mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);

            // Setup the mock IDbCommand to return the mock IDataParameterCollection
            mockDbCommand.Setup(x => x.Parameters).Returns(mockDataParameterCollection.Object);

            IList<donorBLL> donors = new List<donorBLL>
                {
                    new()
                    {
                       donor_id=-1,
                       first_name="test",
                       last_name="test",
                       email="test",
                       contact="test",
                       gender="male",
                       address="test",
                       blood_group="O+",
                       added_date=DateTime.Now,
                       image_name="test",
                       added_by=1

                    }
                };

            if (donors.Any(d => d.donor_id != Donor_id))
            {
                // Setup the mock IDbCommand to return a value of 0 when ExecuteNonQuery is called
                mockDbCommand.Setup(x => x.ExecuteNonQuery()).Returns(0);
            }

            var donor = new donorBLL { donor_id = Donor_id };

            var dal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Act
            var result = dal.Delete(donor);

            // Assert
            return result;
        }

        [Test]
        [TestCase("A+",ExpectedResult ="2",TestName ="CountDonors_ReturnsCorrectValue(A+)")]
        public String CountDonors_ReturnsCorrectValue(String bloodGroup)
        {

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("donor_id", typeof(int));
            table.Columns.Add("first_name", typeof(string));
            table.Columns.Add("last_name", typeof(string));
            table.Columns.Add("email", typeof(string));
            table.Columns.Add("contact", typeof(string));
            table.Columns.Add("gender", typeof(string));
            table.Columns.Add("address", typeof(string));
            table.Columns.Add("blood_group", typeof(string));
 

            table.Rows.Add(1, "John","Smith","john@gmail.com","1234","Male", "abc",bloodGroup);
            table.Rows.Add(2, "Jane","Smith","jane@gmail.com","4567","Male","def",bloodGroup);
            dataSet.Tables.Add(table);
            mockDbDataAdapter.Setup(a => a.Fill(It.IsAny<DataSet>())).Callback<DataSet>(ds => ds.Merge(dataSet));
            var sut = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Act
            var result = sut.countDonors(bloodGroup);
            return result;

        }

        [Test]
        [TestCase("A+", ExpectedResult = "0", TestName = "CountDonors_ReturnsIncorrectValue(A+)")]
        public String CountDonors_ReturnsIncorrectValue(String bloodGroup)
        {

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("donor_id", typeof(int));
            table.Columns.Add("first_name", typeof(string));
            table.Columns.Add("last_name", typeof(string));
            table.Columns.Add("email", typeof(string));
            table.Columns.Add("contact", typeof(string));
            table.Columns.Add("gender", typeof(string));
            table.Columns.Add("address", typeof(string));
            table.Columns.Add("blood_group", typeof(string));


            if ("O+".Equals(bloodGroup))
            {
                table.Rows.Add(1, "John", "Smith", "john@gmail.com", "1234", "Male", "abc", "O+");
                table.Rows.Add(2, "Jane", "Smith", "jane@gmail.com", "4567", "Male", "def", "O+");
                dataSet.Tables.Add(table);
            }
            mockDbDataAdapter.Setup(a => a.Fill(It.IsAny<DataSet>())).Callback<DataSet>(ds => ds.Merge(dataSet));
            var sut = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Act
            var result = sut.countDonors(bloodGroup);
            return result;

        }


    }
}


