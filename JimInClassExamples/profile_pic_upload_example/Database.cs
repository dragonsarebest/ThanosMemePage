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
            conn = new System.Data.SQLite.SQLiteConnection("Data Source="+dbpath+";New=False");
            conn.Open();
        }

        //initialize database to known state
        public void Initialize(){
            SQLiteCommand cmd;
            try{
                cmd = new SQLiteCommand("drop table accounts;",conn);
                cmd.ExecuteNonQuery();
            } catch(Exception ){
            }

            cmd = new SQLiteCommand(
                          "create table accounts (email text unique, password text, realname text, uid integer primary key, pic blob)",
                          conn);
            cmd.ExecuteNonQuery();

            //make a dummy account for testing
            AddRecord("test@example.com","real name","abc");

        }

        public bool AddRecord( string email, string realname, string password){
            var cmd = new SQLiteCommand(
                          "insert into accounts (email,realname,password) values ($email, $realname, $password)",
                          conn);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$realname", realname);
            cmd.Parameters.AddWithValue("$password", password);
            try{
                cmd.ExecuteNonQuery();  
                return true;
            } catch(Exception e){
                Console.WriteLine("SQL Made a BooBoo");
                Console.WriteLine(e);
                return false;
            }
        }

        public int getUid(string email,string password){
            var cmd = new SQLiteCommand("select uid from accounts where email=$e and password=$p",conn);
            cmd.Parameters.AddWithValue("$e", email);
            cmd.Parameters.AddWithValue("$p", password);
            using(var R = cmd.ExecuteReader()) {
                while( R.Read() ){
                    long u = (long) R["uid"];
                    return (int)u;
                }
            }
            return -1;
        }

        public bool setBlob( string column, int uid, byte[] blob){
            var cmd = new SQLiteCommand(
                "update accounts set "+column+"=$b where uid=$u",
                            conn);
            cmd.Parameters.AddWithValue("$b", blob);
            cmd.Parameters.AddWithValue("$u", uid);
            try{
                cmd.ExecuteNonQuery();
                return true;
            } catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }

        public byte[] getBlob( string column, int uid){
            var cmd = new SQLiteCommand("select " + column + " from accounts where uid=$u", conn);
            cmd.Parameters.AddWithValue("$u", uid);
            using(var R = cmd.ExecuteReader()) {
                while( R.Read() ){
                    var x = R[column];
                    if(x.GetType() == typeof(DBNull) ) {
                        return null;
                    } else {
                        return (byte[])x;
                    }
                }
            }
            return null;
        }
    }
}

