using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tddtest
{
    class Handler : BlueberryPie.Handler
    {
        static Database db = new Database();

        public static void initDatabase()
        {
            db.Initilize();
        }

        [BlueberryPie.Expose]
        public string doRegister(string email, string password, string realname)
        {
            if (db.AddRecord("accounts", email, realname, password))
                return "CREATED";
            else
                return "FAILED";
        }

        // TODO: byte[] doesn't work. Convert images to bytes
        // https://www.codeproject.com/Articles/196618/C-SQLite-Storing-Images
        // http://zetcode.com/db/sqlitecsharp/images/
        [BlueberryPie.Expose]
        public string doUploadMeme(byte[] memeData)
        {
            if (db.UploadMeme("posts", memeData))
                return "UPLOADED";
            else
                return "FAILED TO UPLOAD";
        }
    }

    class MainClass
    {
        static BlueberryPie.Server<Handler> srv;

        static public void Main()
        {
            var str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (str.StartsWith("file:"))
                str = str.Substring(6);
            Directory.SetCurrentDirectory(str);
            srv = new BlueberryPie.Server<Handler>(port: 8888, staticFileDir: "../../../html");
            srv.StartInBackground();
            Console.WriteLine("Server Started");
        }

        static public void Dispose()
        {
            srv.Dispose();
        }
    }
}
