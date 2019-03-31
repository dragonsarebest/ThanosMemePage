using System;
using System.IO;

namespace Main
{
    class Handler : BlueberryPie.Handler
    {
        public static Database db = new Database();
        // OUR CODE!!!!

        //add new data to the account tables
        [BlueberryPie.Expose]
        public string addRecord(string username, string email, string password)
        {
            if (db.AddRecord(email, username, password))
            {
                Handler.db.printAccountTables();
                return "CREATED";
            }
            return "FAILED";
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            //always clear the database on startup
            Handler.db.Initialize();              //Only run this if you want to reset the database
            Handler.db.printAccountTables();        //Print the users in the database inside the Accounts table


            var srv = new BlueberryPie.Server<Handler>(port: 8888, staticFileDir: "..\\..\\..\\html");
            srv.Start();
        }
    }
}

