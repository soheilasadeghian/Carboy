
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

    public class getUserMessageListConstructor : AuthMethod
    {
        private const int timeOutInSecond = 300;
        public long sessionID;
        public long userID;
        public string filter;
        public int pageIndex;
        public int count;


        public getUserMessageListConstructor()
        {
        }
        public getUserMessageListConstructor(long sessionID, long userID, string filter, int pageIndex, int count)
        {
            this.filter = filter;
            this.pageIndex = pageIndex;
            this.count = count;
            this.userID = userID;
            this.sessionID = sessionID;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            filter = Serializers.String.read(reader);
            pageIndex = reader.ReadInt32();
            count = reader.ReadInt32();
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

            string result = eng.getUserMessageList(se.userID, filter, pageIndex, count);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);

        }



    }
}