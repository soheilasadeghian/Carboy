using MBProtoLib.Core;
using MBProtoLib.Utils;
using MBProtoLib.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
namespace CarBoyWebservice.MBProto.AuthObjectCollection
{

    public class LogoutConstructor : AuthMethod
    {
        public long sessionID;

        public LogoutConstructor()
        {
        }
        public LogoutConstructor(long sessionID)
        {
            this.sessionID = sessionID;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
        }

        public override string ToString()
        {
            return "";
        }

        public override byte[] Do()
        {
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new CarBoyWebservice.DataAccessDataContext();
            var se = db.MBProto_user_sessionTbls.Single(c => c.sessionID == sessionID);

            db.MBProto_user_sessionTbls.DeleteOnSubmit(se);
            db.SubmitChanges();

            return UserAuth.GenerateMessage(ConfigurationManager.AppSettings["crypto"].ToString());
        }

    }
}