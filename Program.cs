using System;
using System.Data.SQLite;

namespace sqlitetest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string sqlfile = "someFilename.xyz";
            System.IO.File.Delete(sqlfile);
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + sqlfile + ";New=False");
            conn.Open();


            string[] s = {

                //creation of our 6 tables
                "create table users (uid integer primary key, email text, name text, password blob, profilePic blob, isAdmin integer);",
                "create table Tag (uid integer primary key,  content text);",
                "create table comment (uid integer, postid integer, currentid integer primary key, parentid integer, date integer, comment text);",
                "create table rating (uid integer foreign key, postid integer foreign key, rating integer);",
                "create table view (ip text, userid integer foreign key, postid integer foreign key, date integer);",
                "create table posts (creatorID integer, postID integer primary key, isWorldVisible integer, postDate integer, content blob);",


                //testing the 6 tables
                "insert into users (uid, email, name, isAdmin) values (0, 'Alex@example.com', 'Alex', 1)",
                "insert into Tag (key,content) values(0, 'fresh')",
                "insert into comment (uid, postid, currentid, parentid, date, comment) values (0, 0, 0, NULL, date('now'), 'Test to see if we can comment!')",
                "insert into rating (uid,postid,rate) values (100, 20, 5)",
                "insert into view(ip, userid, postid, date) values('123.456.7.8', 0, 0, 20190213)",
                "insert into posts (creatorID,postID,isWorldVisible,postDate,content) values (0,0,1,2019213,NULL)",
            };

            SQLiteCommand cmd;

            foreach (string c in s)
            {
                cmd = new SQLiteCommand(c, conn);
                cmd.ExecuteNonQuery();
            }

            
            cmd = new SQLiteCommand("select name from users where uid = 0;", conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT: " + rdr["name"]);
            }

            Console.WriteLine("--------------------");
            /*
            cmd = new SQLiteCommand("select name,uid from users where name = $name;", conn);
            cmd.Parameters.AddWithValue("$name", "alice");
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT 2: " + rdr["name"] + " " + rdr["uid"]);
            }
            Console.WriteLine("--------------------");

            //assume n is some string
            cmd = new SQLiteCommand("select users.name as BLAH, count(posts.id) as BOOM from users " +
            "left outer join posts on users.uid=posts.uid group by users.name " +
            "order by count(posts.id) desc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("OUTER JOIN RESULT: " + rdr["BLAH"] + " " + rdr["BOOM"]);
            }
            */
            conn.Close();
            Console.ReadLine();
        }
    }
}
