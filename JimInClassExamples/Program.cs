using System;
using System.Data.SQLite;

namespace ourCodes
{
    class Program
    {
        public static void Main()
        {
            var sv = new BlueberryPie.Server<BlueberryPie.Handler>
            (
                port:8888, staticFileDir: "../../../files"
            );

            sv.StartInBackground();
        }
    }
}
