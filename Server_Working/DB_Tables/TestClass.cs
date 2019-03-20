using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tddtest
{
    [TestFixture]
    class TestClass
    {
        string serverurl = "http://localhost:8888/";

        [OneTimeSetUp]
        public void setUpAllTheThings()
        {
            MainClass.Main();
        }

        [OneTimeTearDown]
        public void tearDownAllThetings()
        {
            MainClass.Dispose();
        }

        [SetUp]
        public void testSetupEveryTime()
        {
            Handler.initDatabase();
        }

        [Test]
        public void testGetRegistrationPage()
        {
            WebClient wc = new WebClient();
            string s = wc.DownloadString("http://localhost:8888/signup.html");
            Console.WriteLine(s);
            Assert.AreNotEqual(-1, s.IndexOf("Sign Up"));
        }

        [Test]
        public void testSubmitData()
        {
            WebClient wc = new WebClient();
            var nvc = new NameValueCollection();
            nvc.Add("email", "bob@example.com");
            nvc.Add("password", "s3cr3t");
            nvc.Add("realname", "bobbothy senior");
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
            nvc.Add("realname", "bobbothy senior");
            byte[] resp = wc.UploadValues(serverurl + "doRegister", nvc);
            string s = System.Text.Encoding.UTF8.GetString(resp);
            Console.WriteLine(s);
            Assert.AreEqual("CREATED", s);

            nvc = new NameValueCollection();
            nvc.Add("email", "bob@example.com");
            nvc.Add("password", "s3cr3tiv3");
            nvc.Add("realname", "bobbothy junior");
            resp = wc.UploadValues(serverurl + "doRegister", nvc);
            s = System.Text.Encoding.UTF8.GetString(resp);
            Console.WriteLine(s);
            Assert.AreEqual("FAILED", s);
        }
    }
}
