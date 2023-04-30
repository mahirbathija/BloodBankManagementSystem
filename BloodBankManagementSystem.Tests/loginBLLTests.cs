using NUnit.Framework;
using BloodBankManagementSystem.BLL;

namespace BloodBankManagementSystem.Tests.BLL
{
    [TestFixture]
    public class loginBLL_Tests
    {
        [Test]
        [TestCase("username", "password")]
        [TestCase("", "password")]
        [TestCase("username", "")]
        [TestCase("", "")]
        [TestCase("jackjohn@yahoo.com", "password438")]
        [TestCase("", "password348")]
        [TestCase("matthewstephen@gmail.com","")]
        public void loginBLL_Initialize(string username, string password)
        {
            // Arrange
            loginBLL login = new loginBLL();

            // Act
            login.username = username;
            login.password = password;

            // Assert
            Assert.AreEqual(login.username, username);
            Assert.AreEqual(login.password, password);
        }
    }
}
