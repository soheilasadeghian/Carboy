
using CarBoyWebservice;
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

    public class GetUserUnreadMessageCountConstructor : AuthMethod
    {
        private const int timeOutInSecond = 300;
        public long sessionID;

        public GetUserUnreadMessageCountConstructor()
        {
        }
        public GetUserUnreadMessageCountConstructor(long sessionID)
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

            var eng = new Engine();

            var db = new CarBoyWebservice.DataAccessDataContext();
            var se = db.MBProto_user_sessionTbls.Single(c => c.sessionID == sessionID);

            string result = eng.getUserUnreadMessageCount(se.userID);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);

        }

    }
}