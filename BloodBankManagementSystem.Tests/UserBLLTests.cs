using NUnit.Framework;
using BloodBankManagementSystem.BLL;

namespace BloodBankManagementSystem.Test
{
    [TestFixture]
    public class userBLLTest
    {
        [Test]
        [TestCase(1, "MatthewStephen", "matthewstephen@gmail.com", "password090", "Matthew Stephen", "49745673456", "12 Madison St", "2023-05-19", "matthew.jpg")]
        [TestCase(2, "JackJohn", "jackjohn@yahoo.com", "password438", "JackJohn", "9987234435", "56 Marina St", "2023-04-30", "jack.jpg")]
        [TestCase(3,"JatinRathore","jatin@gmail.com", "password348", "JatinRathore","8865438898","97 Anderson St","2023-05-12","jatin.jpg")]
        public void userBLLConstructorTest(int user_id, string username, string email, string password, string full_name, string contact, string address, string added_date, string image_name)
        {
            userBLL user = new userBLL()
            {
                user_id = user_id,
                username = username,
                email = email,
                password = password,
                full_name = full_name,
                contact = contact,
                address = address,
                added_date = DateTime.Parse(added_date),
                image_name = image_name
            };

            Assert.AreEqual(user_id, user.user_id);
            Assert.AreEqual(username, user.username);
            Assert.AreEqual(email, user.email);
            Assert.AreEqual(password, user.password);
            Assert.AreEqual(full_name, user.full_name);
            Assert.AreEqual(contact, user.contact);
            Assert.AreEqual(address, user.address);
            Assert.AreEqual(DateTime.Parse(added_date), user.added_date);
            Assert.AreEqual(image_name, user.image_name);
        }
    }
}
