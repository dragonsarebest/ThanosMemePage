using System;
using System.IO;
using System.Reflection;

namespace tdd_exercise
{
    class Handler : BlueberryPie.Handler
    {
        static Database db = new Database();

        // for test harness
        public static void initializeDatabase()
        {
            db.initialize();
        }

        [BlueberryPie.Expose]
        public string doRegister(string email, string password, string realname)
        {
            if (db.AddRecord(email, realname, password))
                return "CREATED";
            else
                return "FAILED";
        }
    }
    class Program
    {
        static BlueberryPie.Server<Handler> srv;
        public static void Main()
        {
            string s = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (s.StartsWith("file:"))
                s = s.Substring(6);
            System.IO.Directory.SetCurrentDirectory(s);
            srv = new BlueberryPie.Server<Handler>(port: 8888, staticFileDir: "../../../html");
            Console.WriteLine("Listening on 8888");
            srv.StartInBackground();
        }
        public static void Dispose()
        {
        }
    }
}
