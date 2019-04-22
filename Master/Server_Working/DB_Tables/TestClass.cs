using System;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;

namespace tddtest
{
    [TestFixture]
    public class MyTests
    {
        string serverurl = "http://localhost:8888/";

        public MyTests()
        {
        }

        [OneTimeSetUp]
        public void setUpAllTheThings()
        {
            //Main.MainClass();
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

            Assert.IsFalse(Main.Handler.db.AddRecord(username, email, password));
        }

        [Test]
        public void TestLogOut()
        {

        }
    }
}

