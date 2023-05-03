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

            // Setup the mock IDbCommand to return a value of 1 when ExecuteNonQuery is called
            mockDbCommand.Setup(x => x.ExecuteNonQuery()).Returns(1);

            var donor = new donorBLL { donor_id = Donor_id };

            var dal = new donorDAL(mockDbConnection.Object,mockDbDataAdapter.Object);

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

            // Setup the mock IDbCommand to return a value of 1 when ExecuteNonQuery is called
            mockDbCommand.Setup(x => x.ExecuteNonQuery()).Returns(0);

            var donor = new donorBLL { donor_id = Donor_id };

            var dal = new donorDAL(mockDbConnection.Object, mockDbDataAdapter.Object);

            // Act
            var result = dal.Delete(donor);

            // Assert
            return result;
        }





    }
}


