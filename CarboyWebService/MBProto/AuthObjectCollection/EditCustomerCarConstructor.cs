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

    public class EditCustomerCarConstructor : AuthMethod
    {
        public long sessionID;
        public long userID;
        public string serviceCode;
        public int kilometer;

        public EditCustomerCarConstructor()
        {

        }
        public EditCustomerCarConstructor(long sessionID, long userID, string serviceCode, int kilometer)
        {
            this.sessionID = sessionID;
            this.userID = userID;
            this.serviceCode = serviceCode;
            this.kilometer = kilometer;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            serviceCode = Serializers.String.read(reader);
            kilometer = reader.ReadInt32();
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
            var result = eng.editCustomerCar(se.userID, serviceCode, kilometer);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);
        }

    }
}