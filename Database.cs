using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Main
{
    public class Database
    {
        static readonly string dbpath = "database.sql";
        System.Data.SQLite.SQLiteConnection conn;
        public Database()
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
            }
            catch (Exception)
            {
            }

            cmd = new SQLiteCommand(
                          "create table accounts (email text unique, password text, username text, uid integer primary key)",
                          conn);
            cmd.ExecuteNonQuery();

            //make a dummy account for testing
            AddRecord("test@example.com", "real name", "abc");

        }

        public int getUid(string email, string password)
        {
            var cmd = new SQLiteCommand("select uid from accounts where email=$e and password=$p", conn);
            cmd.Parameters.AddWithValue("$e", email);
            cmd.Parameters.AddWithValue("$p", password);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    long u = (long)R["uid"];
                    return (int)u;
                }
            }
            return -1;
        }

        public bool setBlob(string column, int uid, byte[] blob)
        {
            var cmd = new SQLiteCommand(
                "update accounts set " + column + "=$b where uid=$u",
                            conn);
            cmd.Parameters.AddWithValue("$b", blob);
            cmd.Parameters.AddWithValue("$u", uid);
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

        public byte[] getBlob(string column, int uid)
        {
            var cmd = new SQLiteCommand("select " + column + " from accounts where uid=$u", conn);
            cmd.Parameters.AddWithValue("$u", uid);
            using (var R = cmd.ExecuteReader())
            {
                while (R.Read())
                {
                    var x = R[column];
                    if (x.GetType() == typeof(DBNull))
                    {
                        return null;
                    }
                    else
                    {
                        return (byte[])x;
                    }
                }
            }
            return null;
        }




        // OUR CODE!!!
        
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

