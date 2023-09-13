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

    public class registerUserLocationConstructor : AuthMethod
    {
        public long sessionID;
        public long userID;
        public double latitude, longitude;

        public registerUserLocationConstructor()
        {

        }
        public registerUserLocationConstructor(long sessionID,long userID,double latitude,double longitude)
        {
            this.sessionID = sessionID;
            this.userID = userID;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override void Read(BinaryReader reader)
        {
            sessionID = reader.ReadInt64();
            latitude = reader.ReadDouble();
            longitude = reader.ReadDouble();
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
            var result = eng.registerUserLocation(se.userID, latitude, longitude);

            return MBProtoLib.Core.UserAuth.MakeResponse(ConfigurationManager.AppSettings["crypto"].ToString(), se.userID, se.sessionID, se.diffKey, result);

        }

    }
}