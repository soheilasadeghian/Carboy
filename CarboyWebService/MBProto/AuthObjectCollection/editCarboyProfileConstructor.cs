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

    public class editCarboyProfileConstructor : AuthMethod
    {
        public long sessionID;
        public long userID;
        public string oldPassword;
        public string newPassword;

        public editCarboyProfileConstructor()
        {

        }
        public editCarboyProfileConstructor(long sessionID, long userID, string oldPassword,string newPassword)
        {
            this.sessionID = sessionID;
            this.userID = userID;
            this.oldPassword = oldPassword;
            this.newPassword = newPassword;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            oldPassword = Serializers.String.read(reader);
            newPassword = Serializers.String.read(reader);
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
            var result = eng.editCarboyProfile(se.userID, oldPassword,newPassword);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);
        }

    }
}