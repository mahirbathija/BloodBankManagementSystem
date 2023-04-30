using System;
using NUnit.Framework;
using BloodBankManagementSystem.BLL;

namespace BloodBankManagementSystem.Tests.BLL
{
    [TestFixture]
    public class DonorBLLTests
    {
        [Test]
        [TestCase(1, "Ralph", "Chow", "ralph@gmail.com", "46794567890", "Male", "12 Trevor St", "A+", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "", "Chow", "ralph@example.com", "46794567890", "Male", "12 Trevor St", "A +", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "", "ralph@example.com", "46794567890", "Male", "12 Trevor St", "A+", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "", "1234567890", "Male", "12 Trevor St", "A+", "2023-04-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "", "Male", "12 Trevor St", "A+", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "46794567890", "", "12 Trevor St", "A+", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "46794567890", "Male", "", "A+", "2023-04-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "46794567890", "Male", "12 Trevor St", "", "2023-05-29", "image.jpg", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "46794567890", "Male", "12 Trevor St", "A+", "2023-05-29", "", 1)]
        [TestCase(1, "Ralph", "Chow", "ralph@example.com", "46794567890", "Male", "12 Trevor St", "A+", "2023-05-29", "image.jpg", 0)]
        public void donorBLL_Initialization(int donor_id, string first_name, string last_name, string email, string contact, string gender, string address, string blood_group, string added_date, string image_name, int added_by)
        {
            // Arrange
            donorBLL donor = new donorBLL();

            // Act
            donor.donor_id = donor_id;
            donor.first_name = first_name;
            donor.last_name = last_name;
            donor.email = email;
            donor.contact = contact;
            donor.gender = gender;
            donor.address = address;
            donor.blood_group = blood_group;
            donor.added_date = DateTime.Parse(added_date);
            donor.image_name = image_name;
            donor.added_by = added_by;

            // Assert
            Assert.AreEqual(donor.donor_id, donor_id);
            Assert.AreEqual(donor.first_name, first_name);
            Assert.AreEqual(donor.last_name, last_name);
            Assert.AreEqual(donor.email, email);
            Assert.AreEqual(donor.contact, contact);
            Assert.AreEqual(donor.gender, gender);
            Assert.AreEqual(donor.address, address);
            Assert.AreEqual(donor.blood_group, blood_group);
            Assert.AreEqual(donor.added_date,(DateTime.Parse(added_date)));
            Assert.AreEqual(donor.image_name, image_name);
            Assert.AreEqual(donor.added_by, added_by);

        }
    }
  }
 
 