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

    public class registerUserPushConstructor : AuthMethod
    {
        private const int timeOutInSecond = 300;
        public long sessionID;
        public string token;
        public string platform;

        public registerUserPushConstructor()
        {
        }
        public registerUserPushConstructor(long sessionID, string token,string platform)
        {
            this.sessionID = sessionID;
            this.token = token;
            this.platform = platform;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            token = Serializers.String.read(reader);
            platform = Serializers.String.read(reader);

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
            var result = eng.registerUserPush(se.userID,token, platform);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);

        }



    }
}