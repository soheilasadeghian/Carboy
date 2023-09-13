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

    public class getCarboyHistoryServiceListConstructor : AuthMethod
    {
        public long sessionID;
        public long userID;
        public int pageIndex;
        public int pageCount;


        public getCarboyHistoryServiceListConstructor()
        {

        }
        public getCarboyHistoryServiceListConstructor(long sessionID, long userID, int pageIndex,int pageCount)
        {
            this.sessionID = sessionID;
            this.userID = userID;
            this.pageIndex = pageIndex;
            this.pageCount = pageCount;

        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            pageIndex = reader.ReadInt32();
            pageCount = reader.ReadInt32();
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
            var result = eng.getCarboyHistoryServiceList(se.userID,pageIndex,pageCount);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);
        }

    }
}