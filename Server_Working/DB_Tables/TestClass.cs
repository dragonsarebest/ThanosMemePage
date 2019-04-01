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
            //Handler.InitializeDatabase();
        }

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
            nvc.Add("email", "bob@example.com");
            nvc.Add("password", "s3cr3t");
            nvc.Add("realname", "Bob Bob A Rebob");
            byte[] resp = wc.UploadValues(serverurl + "doRegister", nvc);
            string s = System.Text.Encoding.UTF8.GetString(resp);
            Console.WriteLine(s);
            Assert.AreEqual("CREATED", s);
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
            resp = wc.UploadValues(serverurl + "doRegister", nvc);
            s = System.Text.Encoding.UTF8.GetString(resp);
            Assert.AreEqual("FAILED", s);
        }
    }
}

