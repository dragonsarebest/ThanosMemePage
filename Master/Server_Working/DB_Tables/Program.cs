using System;
using System.IO;

namespace Main
{
    class Handler : BlueberryPie.Handler
    {
        public static Database db = new Database();
        // OUR CODE!!!!

        // Add new data to the account tables
        [BlueberryPie.Expose]
        public string addRecord(string username, string email, string password)
        {
            // Calls the addRecord function from the database
            if (db.AddRecord(email, username, password))
            {
                Handler.db.printAccountTables();
                return "CREATED";
            }
            return "FAILED";
        }

        // Adds data to the comments table
        [BlueberryPie.Expose]
        public string addComment(string comment)
        {
            // Calls the addComment function from the database
            if (db.addComment(comment))
            {
                Handler.db.printCommentTables();
                return "CREATED";
            }
            return "FAILED";
        }

        [BlueberryPie.Expose]
        public string uploadMeme(Stream meme)
        {
            int userID = Session.data["uid"];
            if (userID == -1)
            {
                return "ERROR: not logged in";
            }
            var sr = new BinaryReader(meme);
            var data = sr.ReadBytes((int)meme.Length);
            if (db.setBlob("postdata", userID, data))
            {
                //Handler.db.printPostsTables();
                return "CREATED";
            }
            return "FAILED";
        }

        //gets the user ID from the session database
        int getUid()
        {
            if (!Session.data.ContainsKey("uid"))
                return -1;
            return Session.data["uid"];
        }

        //uses canned username/password
        [BlueberryPie.Expose]
        public string doLogin(string email, string password)
        {
            Session.data["uid"] = db.getUid(email, password);
            Console.WriteLine("Uid: " + db.getUid(email, password));
            return "OK";
        }

        // Sets the session data to not contain a session ID
        [BlueberryPie.Expose]
        public string doLogout()
        {
            Session.data["uid"] = -1;
            return "OK";
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            //always clear the database on startup
            //Handler.db.Initialize();              //Only run this if you want to reset the database
            Handler.db.printAccountTables();        //Print the users in the database inside the Accounts table
            Handler.db.printCommentTables();

            var srv = new BlueberryPie.Server<Handler>(port: 8888, staticFileDir: "..\\..\\..\\html");
            srv.Start();
        }
    }
}

