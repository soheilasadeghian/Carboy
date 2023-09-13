using MBProtoLib.Core;
using MBProtoLib.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
namespace CarBoyWebservice.MBProto.AuthObjectCollection
{

    public class distributorRequestCancelConstructor : AuthMethod
    {
        public long sessionID;
        public long userID;

        public distributorRequestCancelConstructor()
        {

        }
        public distributorRequestCancelConstructor(long sessionID, long userID)
        {
            this.sessionID = sessionID;
            this.userID = userID;
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

            var eng = new Engine();
            var result = eng.distributorRequestCancel(se.userID);
            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);

        }

    }
}