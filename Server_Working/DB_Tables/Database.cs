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
        static readonly string dbpath = "database.sql";
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
            cmd = new SQLiteCommand("create table accounts (username text, email text unique, password text, uid integer primary key)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table posts (postid integer primary key, creatorid integer, worldvisible integer, postdata blob, jtagid integer, jcommentid integer)", conn);
            cmd.ExecuteNonQuery();
            // tag junction table
            cmd = new SQLiteCommand("create table jtag (postid integer, tagid integer)", conn);
            cmd.ExecuteNonQuery();
            // comment junction table
            cmd = new SQLiteCommand("create table jcomment (postid integer, commentid integer)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table tags (tagid integer, content string)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table rating (userid integer, postid integer, rating integer)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table views (ip text, userid integer, postid integer, date integer)", conn);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("create table comments (commentid integer, parentid integer, date integer, data text, postid integer, userid integer, jtagid integer)", conn);
            cmd.ExecuteNonQuery();

            //make a dummy account for testing
            AddRecord("test@example.com", "real name", "abc");

        }

        public bool UploadMeme(string table, byte[] memeData)
        {
            var cmd = new SQLiteCommand(
                "insert into " + table + " (postdata) values ($memeData)", conn);
            cmd.Parameters.AddWithValue("$memeData", memeData);
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
            cmd.Parameters.AddWithValue("$password", password);
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

        // Prints out the Account tables for bug testing!
        public void printAccountTables()
        {
            try
            {
                var cmd = new SQLiteCommand("select * from accounts", conn);
                using (var R = cmd.ExecuteReader())
                {
                    while (R.Read())
                    {
                        string name = (string)R["username"];
                        Console.WriteLine(name);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
