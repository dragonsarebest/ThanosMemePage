using System;
using System.IO;

namespace test
{
    class Handler : BlueberryPie.Handler
    {
        [BlueberryPie.Expose(mimetype: "text/plain")]
        public string postDates (string textdata, Stream avatardata)
        {
            return "Your start date is \"" + date1data + "\" and end date is " + date2data;
        }
    }

    class MainClass
    {
        public static void Main (string[] args)
        {
            var srv = new BlueberryPie.Server<Handler>(port:8888, staticFileDir:"files");
            Console.WriteLine("Listening on 8888");
            srv.Start();
        }
    }
}
