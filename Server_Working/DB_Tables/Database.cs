using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tddtest
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

        public void Initilize()
        {
            SQLiteCommand cmd;
            try
            {
                cmd = new SQLiteCommand("drop table accounts", conn);
                cmd.ExecuteNonQuery();
            } catch(Exception e)
            {
            }
            cmd = new SQLiteCommand("create table accounts (email text unique, password text, realname text, uid integer primary key)", conn);
            cmd.ExecuteNonQuery();
        }

        public bool AddRecord(string table, string email, string realname, string password)
        {
            var cmd = new SQLiteCommand(
            "insert into " + table + " (email, realname, password) values ($email, $realname, $password)",
            conn);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$realname", realname);
            cmd.Parameters.AddWithValue("$password", password);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            } catch(Exception e)
            {
                Console.WriteLine("SQL made a booboo");
                Console.WriteLine(e);
                return false;
            }
        }

    }
}
