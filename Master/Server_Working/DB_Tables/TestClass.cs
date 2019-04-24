using System;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;

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

        /*
        [Test]
        public void testGetRegistrationPage()
        {
            WebClient wc = new WebClient();
            string s = wc.DownloadString(serverurl + "signup.html");
            Assert.AreNotEqual(-1, s.IndexOf("Register"));
        }
        
        [Test]
        public void testSubmitData()
        {
            WebClient wc = new WebClient();
            var nvc = new NameValueCollection();
            nvc.Add("username", "Bob Ross");
            nvc.Add("email", "happylittletrees@example.com");
            nvc.Add("password", "mistakes");
            byte[] resp = wc.UploadValues(serverurl + "signup.html", nvc);
            string s = System.Text.Encoding.UTF8.GetString(resp);
            Console.WriteLine("Response: \n\n");
            Console.WriteLine(s);
            Assert.AreEqual("signup.html", s);
        }

        [Test]
        public void testNoDuplicateAccount()
        {
            WebClient wc = new WebClient();
            var nvc = new NameValueCollection();
            nvc.Add("email", "bob@example.com");
            nvc.Add("password", "s3cr3t");
            nvc.Add("realname", "Bob Bob A Rebob");
            byte[] resp = wc.UploadValues(serverurl + "doRegister", nvc);
            string s = System.Text.Encoding.UTF8.GetString(resp);
            Assert.AreEqual("CREATED", s);

            nvc = new NameValueCollection();
            nvc.Add("email", "bob@example.com");
            nvc.Add("password", "s3cr3tiv3");
            nvc.Add("realname", "Alice");
            resp = wc.UploadValues(serverurl + "user", nvc);
            s = System.Text.Encoding.UTF8.GetString(resp);
            Assert.AreEqual("FAILED", s);
        }
        */

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
        public void TestLogOut()
        {

        }

        [Test]
        public void TestPrintingCommentTables()
        {
            //this isn't a super important test, but it should never fail
            //I just wanted to see if the database was empty
            db.printCommentTables();
        }
    }
}

