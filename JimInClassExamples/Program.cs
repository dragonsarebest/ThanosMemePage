using System;
using System.Data.SQLite;
using System.Reflection;

namespace ourCodes
{
    class Program
    {

        public static BlueberryPie.Server<BlueberryPie.Handler> sv;

        public static void Main()
        {
            string s = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (s.StartsWith(@"file:\"))
            {
                s = s.Substring(6);
            }
            System.IO.Directory.SetCurrentDirectory(s);

            sv = new BlueberryPie.Server<BlueberryPie.Handler>
            (
                port:8888, staticFileDir: "../../../files"
            );

            sv.StartInBackground();
        }

        public static void dispose()
        {
            sv.Dispose();
        }
    }
}
