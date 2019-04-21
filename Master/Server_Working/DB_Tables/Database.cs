using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Database
    {
        //HashSet<string> knownEmails = new HashSet<string>();
        static readonly string dbpath = "..\\..\\database.sql";
        System.Data.SQLite.SQLiteConnection conn;
        public Database()
        {
            OpenIt();
        }

        void OpenIt()
        {
            conn = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbpath + ";New=False");
            conn.Open();
        }

        //initialize database to known state
        public void Initialize()
        {
            SQLiteCommand cmd;
            try
            {
                // Completly wipes all tables from the database
                cmd = new SQLiteCommand("drop table accounts;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table posts;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table jtag;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table jcomment;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table tags;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table rating;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table views;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand("drop table comments;", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }

            // TODO: change the way passwords are saved
            cmd = new SQLiteCommand("create table accounts (username text, email text unique, password long, uid integer primary key)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table posts (postid integer primary key, creatorid integer, worldvisible integer, postdata blob, date integer)", conn);
            cmd.ExecuteNonQuery();
            // tag junction table
            cmd = new SQLiteCommand("create table jtag (postid integer, tagid integer)", conn);
            cmd.ExecuteNonQuery();
            // comment junction table
            cmd = new SQLiteCommand("create table jcomment (postid integer, commentid integer)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table tags (tagid integer, content string unique)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table rating (userid integer, postid integer, rating integer)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table views (ip text, userid integer, postid integer, date integer)", conn);
            cmd.ExecuteNonQuery();
            //cmd = new SQLiteCommand("create table comments (commentid integer, parentid integer, date integer, comment text, postid integer, userid integer)", conn);
            //cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table comments (comment text, commentid integer primary key)", conn);
            cmd.ExecuteNonQuery();

            //make a dummy account for testing
            AddRecord("test@example.com", "bob bobby bobbity boop", "abc");
            addComment("test comment");
            addTag("your mom oooooh");
            addTag("your father smelled of elderberries");
            addTag("no you");
            addTag("owo whats this");

        }

        public bool addTag(string tag)
        {
            var cmd = new SQLiteCommand(
                "insert into tags (content) values ($content)", conn);
            cmd.Parameters.AddWithValue("$content", tag);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL made a booboo");
                Console.WriteLine(e);
                return false;
            }
        }

        public string getAllTags()
        {
            string tags = "";
            var cmd = new SQLiteCommand("select content from tags", conn);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    string u = (string)R["content"];
                    tags += u + ",";
                }
            }
            Console.WriteLine(tags);
            return tags;
        }

        // Adds a new Comment to the database
        public bool addComment(string comment)
        {
            var cmd = new SQLiteCommand(
                "insert into comments (comment) values ($comment)", conn);
            cmd.Parameters.AddWithValue("$comment", comment);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL made a booboo");
                Console.WriteLine(e);
                return false;
            }
        }

        public bool UploadMeme(byte[] memeData, int userID)
        {
            var cmd = new SQLiteCommand(
                "insert into posts (creatorid, worldvisible, postdata, date) values ($creatorid, $worldvisible, $postdata, $date)", conn);
            cmd.Parameters.AddWithValue("$creatorid", userID);
            cmd.Parameters.AddWithValue("$worldvisible", 1);
            cmd.Parameters.AddWithValue("$postdata", memeData);
            cmd.Parameters.AddWithValue("$date", 20190408);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL made a booboo");
                Console.WriteLine(e);
                return false;
            }
        }

        // Adds a new record to the accounts table this will need to go in the real database.cs
        public bool AddRecord(string email, string username, string password)
        {
            var cmd = new SQLiteCommand(
                          "insert into accounts (email,username,password) values ($email, $username, $password)",
                          conn);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$username", username);
            long hash = password.GetHashCode();
            cmd.Parameters.AddWithValue("$password", hash);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL Made a BooBoo");
                Console.WriteLine(e);
                return false;
            }
        }

        // updates old account
        public bool UpdateRecord(string email, string username, string password, string oldPassword, int uid)
        {
            //update Email
            var cmd = new SQLiteCommand("UPDATE accounts SET username = @nu, password = @np, email = @ne Where uid = @u", conn);
            cmd.Parameters.AddWithValue("@nu", username);
            cmd.Parameters.AddWithValue("@np", password);
            cmd.Parameters.AddWithValue("@ne", email);
            cmd.Parameters.AddWithValue("@u", uid.ToString());
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL Made a BooBoo");
                Console.WriteLine(e);
                return false;
            }
        }


        public List<int> getMostRecentPosts()
        {
            List<int> L = new List<int>();
            var cmd = new SQLiteCommand("select postid from posts order by date desc limit 10", conn);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    int p = Convert.ToInt32(R["postid"]);
                    L.Add(p);
                }
            }
            return L;
        }


        // Gets the Uid from the database that matches the sent in email and password
        // Good example to see how to save information from the database!!!
        public int getUid(string email, string password)
        {
            var cmd = new SQLiteCommand("select uid from accounts where email=$e and password=$p", conn);
            cmd.Parameters.AddWithValue("$e", email);
            long hash = password.GetHashCode();
            cmd.Parameters.AddWithValue("$p", hash);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    long u = (long)R["uid"]; //whatever you put in the quotes that is part of that table will go into R
                    return (int)u;
                }
            }
            return -1;
        }

        public string getUsername(string email, string password)
        {
            var cmd = new SQLiteCommand("select username from accounts where email=$e and password=$p", conn);
            cmd.Parameters.AddWithValue("$e", email);
            long hash = password.GetHashCode();
            cmd.Parameters.AddWithValue("$p", hash);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    Console.WriteLine(R["username"]);

                    string name = (string)R["username"]; // Only prints out the username (can change what is in the quotes)
                    return name;
                }
            }
            return "not logged in";
        }

        public string getEmail(string email, string password)
        {
            var cmd = new SQLiteCommand("select Email from accounts where email=$e and password=$p", conn);
            cmd.Parameters.AddWithValue("$e", email);
            long hash = password.GetHashCode();
            cmd.Parameters.AddWithValue("$p", hash);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    string name = (string)R["Email"]; // Only prints out the username (can change what is in the quotes)
                    return name;
                }
            }
            return "not logged in";
        }

        public bool setBlob(string column, int creatorid, byte[] blob)
        {
            var cmd = new SQLiteCommand(
                "update posts set " + column + "=$b where creatorid=$cid", conn);
            cmd.Parameters.AddWithValue("$b", blob);
            cmd.Parameters.AddWithValue("$cid", creatorid);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        // Pulls meme image from database based on post id, check if it exists, and returns it.
        public byte[] GetMeme(int pid)
        {
            var cmd = new SQLiteCommand("select postdata from posts where postid=$p", conn);
            cmd.Parameters.AddWithValue("$p", pid);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    var meme = R["postdata"];
                    if (meme.GetType() == typeof(DBNull))
                    {
                        return null;
                    }
                    else
                    {
                        return (byte[])meme;
                    }
                    //byte[] meme = (byte[])R["postdata"];
                }
            }
            return null;
        }

        // Prints out the Account tables for bug testing!
        public void printAccountTables()
        {
            try
            {
                // Selects all entrys in the accounts table
                var cmd = new SQLiteCommand("select * from accounts", conn);
                using (var R = cmd.ExecuteReader())
                {
                    while (R.Read())
                    {
                        string name = (string)R["username"]; // Only prints out the username (can change what is in the quotes)
                        Console.WriteLine(name);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        // Prints out the comments tables for bug testing!
        public void printCommentTables()
        {
            try
            {
                var cmd = new SQLiteCommand("select * from comments", conn);
                using (var R = cmd.ExecuteReader())
                {
                    while (R.Read())
                    {
                        string c = (string)R["comment"];
                        Console.WriteLine(c);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
