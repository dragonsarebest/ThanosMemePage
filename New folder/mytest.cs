using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;

namespace tdd_exercise
{
    [TestFixture]
    class MyTests
    {
        string serverurl = "http://localhost:8888/";

        public MyTests()
        {
        }

        [OneTimeSetUp]
        public void setup()
        {
            Program.Main();
        }

        [OneTimeTearDown]
        public void teardown()
        {
            Program.Dispose();
        }

        [SetUp]
        public void testSetUpEveryTime()
        {
            Handler.initializeDatabase();
        }


        [Test]
        public void testgetRegistrationPage()
        {
            // SMOKE TEST - does the magic smoke come out? :)
            WebClient wc = new WebClient();
            string s = wc.DownloadString(serverurl + "register.html");
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
            Assert.AreEqual("CREATED", s);
        }

        [Test]
        public void testNoDuplicateAccount()
        {
            // the only thing the same between these 2 is the email
            // checks for duplicate accounts
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
