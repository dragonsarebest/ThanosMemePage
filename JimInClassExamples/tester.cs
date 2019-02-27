﻿using System;
using System.Data.SQLite;
using NUnit.Framework;
using System.Net;

namespace testerNamespace
{
    [TestFixture]
    class tester
    {

        string serverUrl = "http://localhost:8888/";
        bool hasSeen = false;

        [OneTimeSetUp]
        public void setUpAllTheThings()
        {
            ourCodes.Program.Main();
            hasSeen = true;
        }

        [Test]
        public void testGetRegistrationPage()
        {
            TestContext.Out.WriteLine(System.IO.Directory.GetCurrentDirectory());

            using (WebClient wc = new WebClient())
            {
                Console.WriteLine(serverUrl + "login.html");
                try
                {
                    string s = wc.DownloadString(serverUrl + "login.html");
                    TestContext.Out.WriteLine(s);
                    Assert.AreNotEqual(-1, s.IndexOf("Register"));
                }
                catch (Exception e)
                {
                    TestContext.Out.WriteLine(e);
                }

            }
        }
    }
}