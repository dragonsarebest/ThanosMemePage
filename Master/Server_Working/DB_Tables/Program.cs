using System;
using System.Collections.Generic;
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
        public string uploadMeme(Stream meme, string tags)
        {
            int userID = getUid();
            Console.WriteLine("userID == " + userID);
            if (userID == -1)
            {
                return "ERROR: not logged in";
            }
            var sr = new BinaryReader(meme);
            var data = sr.ReadBytes((int)meme.Length);
            string[] tagList = (tags).Split(',');
            if (db.UploadMeme(data, userID, tagList))
            {
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


		// For in-session fetches
		[BlueberryPie.Expose]
		public string getSessionUid()
		{
			if (!Session.data.ContainsKey("uid"))
                return "-1";
            return Session.data["uid"].ToString();
		}

        [BlueberryPie.Expose]
        public string getSessionUsername()
        {
            if (!Session.data.ContainsKey("username"))
                return "not logged in";
            return Session.data["username"].ToString();
        }

        // For in-session fetches
        [BlueberryPie.Expose]
        public string getSessionEmail()
        {
            if (!Session.data.ContainsKey("Email"))
                return "not logged in";
            return Session.data["Email"].ToString();
        }

        //uses canned username/password
        [BlueberryPie.Expose]
        public string doLogin(string email, string password)
        {
            Session.data["uid"] = db.getUid(email, password);
            Session.data["username"] = db.getUsername(email, password);
            Session.data["Email"] = db.getEmail(email, password);
            Console.WriteLine("Uid: " + db.getUid(email, password));
            return "OK";
        }

        [BlueberryPie.Expose]
        public string getAllTags()
        {
            return db.getAllTags();
        }

        [BlueberryPie.Expose]
        public string uploadTags(String tags)
        {
            string[] tagList = (tags).Split(',');
            try
            {
                foreach (string s in tagList)
                {
                    if (s != "")
                    {
                        db.addTag(s);
                    }
                }
            }
            catch (Exception)
            {
                return "FAILURE";
            }

            return "SUCCESS";
        }

        //Updates record of an account
        [BlueberryPie.Expose]
        public string UpdateRecord(string email, string username, string password, string oldPassword, int uid)
        {
            if (db.UpdateRecord(email, username, password, oldPassword, uid))
            {
                Session.data["uid"] = db.getUid(email, password);
                Session.data["username"] = db.getUsername(email, password);
                Session.data["Email"] = db.getEmail(email, password);
                return "CREATED";
            }
            return "FAILED";
        }

        //Updates record of an account without updating password
        [BlueberryPie.Expose]
        public string UpdateRecordNoPassword(string email, string username, string password, string oldPassword, int uid)
        {
            if (db.UpdateRecordNoPassword(email, username, oldPassword, uid))
            {
                Session.data["uid"] = db.getUid(email, oldPassword);
                Session.data["username"] = db.getUsername(email, oldPassword);
                Session.data["Email"] = db.getEmail(email, oldPassword);
                return "CREATED";
            }
            return "FAILED";
        }


        // Sets the session data to not contain a session ID
        [BlueberryPie.Expose]
        public string doLogout()
        {
            Session.data["uid"] = -1;
            Session.data["username"] = "Logged out";
            return "OK";
        }

        // Attempts to get meme image from database by post id
        // TODO add error handling if GetMeme() returns null.
        [BlueberryPie.Expose(mimetype: "application/octet-stream")]
        public byte[] doGetMeme(int pid)
        {
            return db.GetMeme(pid);
        }

        // Gets top ten most recent posts based on submission times
        [BlueberryPie.Expose]
        public string topTen()
        {
            List<string> L = new List<string>();
            var p = db.getMostRecentPosts();
            foreach (var pid in p)
            {
                if (Session.data.ContainsKey("uid"))
                {
                    if (Session.data["uid"] != -1)
                    {
                        Add_Rating_Button(L, pid);
                    }
                }
                
                L.Add("<img src='/doGetMeme?pid=" + pid + "'>");
            }
            return string.Join("<br>", L);
        }

        public void Add_Rating_Button(List<string> L,int pid)
        {
            L.Add("<div class='rating'>");
            L.Add("<input type='radio' onclick=doRating(1); name = "+pid+"   id='Rating_1' value = '1' > ");
            L.Add("<input type='radio' onclick=doRating(2); name = "+pid+"  id='Rating_2' value = '2' > ");
            L.Add("<input type='radio' onclick=doRating(3); name = "+pid+"  id='Rating_3' value = '3' > ");
            L.Add("<input type='radio' onclick=doRating(4); name = "+pid+"  id='Rating_4' value = '4' > ");
            L.Add("<input type='radio' onclick=doRating(5); name = " + pid + "  id='Rating_5' value = '5' > ");
            L.Add("</div>");
        }

        [BlueberryPie.Expose]
        public string doRating(string Post_Id, string Rating)
        {

            if (db.doRating(Int32.Parse(Post_Id), Int32.Parse(Rating), Session.data["uid"]))
            {
                return "Added";
            }
            else
            {
                return "Failed";
            }
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            //always clear the database on startup
            //Handler.db.Initialize();              //Only run this if you want to reset the database//
            Handler.db.printAccountTables();        //Print the users in the database inside the Accounts table
            Handler.db.printCommentTables();
            var srv = new BlueberryPie.Server<Handler>(port: 8888, staticFileDir: "..\\..\\..\\html");
            srv.Start();
            
        }
    }
}

