using System;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace tddtest
{
    [TestFixture]
    public class MyTests
    {
        string serverurl = "http://localhost:9888/";
        string databaseLocale = "..\\..\\testDatabase.sql";
        public static Main.Database db;

        [OneTimeSetUp]
        public void setUpAllTheThings()
        {
            //System.IO.Directory.SetCurrentDirectory("C:\\Users\\Chase\\Documents\\MemeRepository\\trunk\\Master\\Server_Working");
            System.IO.Directory.SetCurrentDirectory(System.IO.Directory.GetCurrentDirectory());
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            db = new Main.Database(databaseLocale);
            db.Initialize();
        }

        [OneTimeTearDown]
        public void tearDownAllTheThings()
        {
            //MainClass.Dispose();
        }

        [SetUp]
        public void testSetupEveryTime()
        {
            //Database.InitializeDatabase();
        }

        [Test]
        public void TestAddRecord()
        {
            string username = "Bob Ross";
            string email = "happytrees@example.com";
            string password = "happy_mistakes";

            db.AddRecord(email, username, password);
            var cmd = new System.Data.SQLite.SQLiteCommand("select uid from accounts where email=$e and password=$p and username = $u", db.conn);
            cmd.Parameters.AddWithValue("$e", email);
            cmd.Parameters.AddWithValue("$p", password);
            cmd.Parameters.AddWithValue("$u", username);
            int userID = -1;
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    Console.WriteLine(R);
                    userID = (int)((long)R["uid"]);
                }
            }
            Assert.AreNotEqual(userID, -1);
        }

        [Test]
        public void TestPrintingAccountTables()
        {
            // another test just to check the table isn't empty
            db.AddRecord("testemail1@example.com", "Coolguy1", "thebestpassword1");
            db.AddRecord("testemail2@example.com", "Coolguy2", "thebestpassword2");
            db.AddRecord("testemail3@example.com", "Coolguy3", "thebestpassword3");

            db.printAccountTables();
        }

        [Test]
        public void TestAddingTag()
        {
            string tag1 = "test_1";
            string tag2 = "test_2";
            string tag3 = "test_3";
            Assert.IsTrue(db.addTag(tag1));
            Assert.IsTrue(db.addTag(tag2));
            Assert.IsTrue(db.addTag(tag3));

            string allTags = db.getAllTags();
        }

        [Test]
        public void TestAddingComment()
        {
            string comment1 = "test_1";
            string comment2 = "test_2";
            string comment3 = "test_3";
            Assert.IsTrue(db.addComment(comment1));
            Assert.IsTrue(db.addComment(comment2));
            Assert.IsTrue(db.addComment(comment3));

            db.printCommentTables();
        }

        [Test]
        public void TestGettingUsername()
        {
            string email = "anewemail@example.com";
            string username = "New Guy";
            string password = "Super_Secret_42";
            string email2 = "anewemail2@example.com";
            string username2 = "New Guy 2";
            string password2 = "Super_Secret_43";

            db.AddRecord(email, username, password);
            db.AddRecord(email2, username2, password2);

            string rcUser = db.getUsername(email, password);
            Assert.Equals(rcUser, username);

            rcUser = db.getUsername(email2, password2);
            Assert.Equals(rcUser, username2);

            rcUser = db.getUsername(email, password2);  // account doesn't exist
            Assert.Equals(rcUser, "not logged in");
        }

        [Test]
        public void TestPrintingCommentTables()
        {
            //this isn't a super important test, but it should never fail
            //I just wanted to see if the database was empty
            db.printCommentTables();
        }

        [Test]
        public void TestGettingMostRecentPosts()
        {
            List<int> A = new List<int>();
            A = db.getMostRecentPosts();
            foreach (int num in A)
            {
                Console.WriteLine(num);
            }
            Assert.IsNotNull(A);
        }

        [Test]
        public void checkLogIn()
        {
            db.AddRecord("emailtest@123.com", "usernameTest", "PasswordTest");
            string user = db.getUsername("emailtest@123.com", "PasswordTest");
            Console.WriteLine(user);
            Assert.AreEqual(user, "usernameTest");
        }

        [Test]
        public void FailCheckLogIn()
        {
            //checking to make sure you can't get a username with an email that isn't registered
            string user = db.getUsername("FAKEEMAIL@123.com", "PasswordTest");
            Assert.AreEqual(user, "not logged in");
        }

        [Test]
        public void checkGetEmail()
        {
            db.AddRecord("emailtest@123.com", "usernameTest", "PasswordTest");
            string user = db.getEmail("emailtest@123.com", "PasswordTest");
            Console.WriteLine(user);
            Assert.AreEqual(user, "emailtest@123.com");
        }

        [Test]
        public void FailCheckGetEmail()
        {
            //checking to make sure you can't get a username with an email that isn't registered
            string user = db.getEmail("FAKEEMAIL@123.com", "PasswordTest");
            Assert.AreEqual(user, "not logged in");
        }
    }
}

