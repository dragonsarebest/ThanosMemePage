using System;
using System.IO;

namespace Main
{
    class Handler : BlueberryPie.Handler{
        public static Database db = new Database();

        //gets the user ID from the session database
        int getUid(){
            if(!Session.data.ContainsKey("uid"))
                return -1;
            return Session.data["uid"];
        }

        //set the profile picture
        [BlueberryPie.Expose]
        public string setMyProfilePic(Stream pic){
            int uid = getUid();
            if(uid == -1) {
                //can't set picture if not logged in
                return "ERROR: Not logged in";
            }
            var sr = new BinaryReader(pic);
            var data = sr.ReadBytes((int)pic.Length);
            db.setBlob("pic", uid, data);
            return "OK";
        }


        [BlueberryPie.Expose(mimetype="application/octet-stream")]
        public byte[] getMyProfilePic(string junk){
            int uid = getUid();
            if(uid == -1) {
                //if the user isn't logged in, we use a question mark image
                return getFile("questionmark.png");
            }
            var data = db.getBlob("pic",uid);
            if(data == null) {
                //if the user doesn't have a profile picture, we use an exclamation point image
                return getFile("exclamationpoint.png");
            }
            return data;
        }

        //uses canned username/password
        [BlueberryPie.Expose]
        public string doLogin(){
            Session.data["uid"] = db.getUid("test@example.com","abc");
            return "OK";
        }

        [BlueberryPie.Expose]
        public string doLogout(){
            Session.data["uid"] = -1;
            return "OK";
        }

        //utility function to read a file from the disk
        byte[] getFile(string fname){
            using(var fr = new FileStream(fname, FileMode.Open)) {
                byte[] b = new byte[(int)fr.Length];
                fr.Read(b, 0, b.Length);
                return b;
            }
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            //always clear the database on startup
            Handler.db.Initialize();

            var srv = new BlueberryPie.Server<Handler>(port: 9888, staticFileDir: "html");
            srv.Start();
        }
    }
}
