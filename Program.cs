using System;
using System.IO;

namespace Main
{
    class Handler : BlueberryPie.Handler
    {
        public static Database db = new Database();

        //gets the user ID from the session database EXAMPLE CODE Does not really pull uid's
        int getUid()
        {
            if (!Session.data.ContainsKey("uid"))
                return -1;
            return Session.data["uid"];
        }

        //set the profile picture EXAMPLE CODE
        [BlueberryPie.Expose]
        public string setMyProfilePic(Stream pic)
        {
            int uid = getUid();
            if (uid == -1)
            {
                //can't set picture if not logged in
                return "ERROR: Not logged in";
            }
            var sr = new BinaryReader(pic);
            var data = sr.ReadBytes((int)pic.Length);
            db.setBlob("pic", uid, data);
            return "OK";
        }


        // Jims getMyProfilePic EXAMPLE CODE
        [BlueberryPie.Expose(mimetype = "application/octet-stream")]
        public byte[] getMyProfilePic(string junk)
        {
            int uid = getUid();
            if (uid == -1)
            {
                //if the user isn't logged in, we use a question mark image
                return getFile("questionmark.png");
            }
            var data = db.getBlob("pic", uid);
            if (data == null)
            {
                //if the user doesn't have a profile picture, we use an exclamation point image
                return getFile("exclamationpoint.png");
            }
            return data;
        }

        //uses canned username/password EXAMPLE CODE
        [BlueberryPie.Expose]
        public string doLogin()
        {
            Session.data["uid"] = db.getUid("test@example.com", "abc");
            return "OK";
        }

        [BlueberryPie.Expose]
        public string doLogout()
        {
            Session.data["uid"] = -1;
            return "OK";
        }

        //utility function to read a file from the disk EXAMPLE CODE
        byte[] getFile(string fname)
        {
            using (var fr = new FileStream(fname, FileMode.Open))
            {
                byte[] b = new byte[(int)fr.Length];
                fr.Read(b, 0, b.Length);
                return b;
            }
        }






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
            //Handler.db.Initialize();              //Only run this if you want to reset the database
            Handler.db.printAccountTables();        //Print the users in the database inside the Accounts table


            var srv = new BlueberryPie.Server<Handler>(port: 9888, staticFileDir: "..\\..\\html");
            srv.Start();
        }
    }
}
