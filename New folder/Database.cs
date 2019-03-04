using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd_exercise
{
    class Database
    {
        //HashSet<string> knownNames = new HashSet<string>();
        static readonly string dbpath = "database.sql";
        System.Data.SQLite.SQLiteConnection conn;

        public Database()
        {
            openIt();
        }

        void openIt()
        {
            conn = new System.Data.SQLite.SQLiteConnection("Data source=database.sql;New=False");
            conn.Open();
        }

        public void initialize()
        {
            conn.Close();
            System.IO.File.Delete(dbpath);
            openIt();
            //knownNames = new HashSet<string>();
            var cmd = new SQLiteCommand("create table accounts (email text, password text, realname text, uid integer primary key)", conn);
            cmd.ExecuteNonQuery();
        }

        public bool AddRecord(string table, string email, string realname, string password)
        {
            var cmd = new SQLiteCommand("insert into" + table + "(email,realname,password) values ($email,$realname,$password)", conn);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$realname", realname);
            cmd.Parameters.AddWithValue("$password", password);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            //Console.WriteLine("adding" + email + "already there?" + knownNames.Contains(email));
            //return knownNames.Add(email);       // false if it's already there
        }
    }


}
