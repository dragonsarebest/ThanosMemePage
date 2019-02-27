using System;
using System.Data.SQLite;
using System.Reflection;

namespace ourCodes
{
    class Program
    {
        public static void Main()
        {
            string s = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (s.StartsWith(@"file:\"))
            {
                s = s.Substring(6);
            }
            System.IO.Directory.SetCurrentDirectory(s);

            var sv = new BlueberryPie.Server<BlueberryPie.Handler>
            (
                port:8888, staticFileDir: "../../../files"
            );

            sv.StartInBackground();
        }
    }
}
