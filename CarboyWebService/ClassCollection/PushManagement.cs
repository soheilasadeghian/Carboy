using OneSignalSharp.Posting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CarBoyWebservice.ClassCollection
{
    public class PushManagement
    {
        private static string SendPushNotification(string[] to, string body, string title, string link = "", string type = "1")
        {
            try
            {
                string Server_key = ConfigurationManager.AppSettings["fcm-server-key"];
                string senderId = ConfigurationManager.AppSettings["fcm-sender-id"]; ;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    registration_ids = to,
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = "Enabled"
                    },
                    data = new
                    {
                        link = link,
                        type = type,
                        shortText = "پیام جدید"
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", Server_key));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                                return str;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return "ERORO->" + str;
            }
        }


        private static string SendOneSignalPushNotification(string[] to, string body, string title, string link = "", string type = "1")
        {
            try
            {
                string appId = ConfigurationManager.AppSettings["onesignal-appId"];
                string restKey = ConfigurationManager.AppSettings["onesignal-restKey"];
                string userAuth = ConfigurationManager.AppSettings["onesignal-userAuth"];


                OneSignalSharp.OneSignalClient client = new OneSignalSharp.OneSignalClient(appId, restKey, userAuth);

                var op = new Device(new HashSet<string>(to));

                Dictionary<string, string> contentMsgs = new Dictionary<string, string>();
                Dictionary<string, string> headingtMsgs = new Dictionary<string, string>();
                Dictionary<string, string> subtitleMsgs = new Dictionary<string, string>();

                var data = new
                {
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = "Enabled"
                    },
                    data = new
                    {
                        link = link,
                        type = type,
                        shortText = "پیام جدید"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);

                contentMsgs = new Dictionary<string, string>();
                contentMsgs.Add("en", body);
                contentMsgs.Add("fa", body);
                contentMsgs.Add("ar", body);

                headingtMsgs = new Dictionary<string, string>();
                headingtMsgs.Add("en", title);
                headingtMsgs.Add("fa", title);
                headingtMsgs.Add("ar", title);

                subtitleMsgs = new Dictionary<string, string>();
                subtitleMsgs.Add("en", "پیام جدید");
                subtitleMsgs.Add("fa", "پیام جدید");
                subtitleMsgs.Add("ar", "پیام جدید");

                ContentAndLanguage content = new ContentAndLanguage(contentMsgs, headingtMsgs, subtitleMsgs, true);
                return client.SendNotification(op, content, new Attachment() { url = link }, null);

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return "ERORO->" + str;
            }
        }

        public static string Send(string[] to, string body, string title, string link = "", string type = "1")
        {
            return SendPushNotification(to, body, title, link, type);
        }

        public static string SendToUser(long userID, string body, string title, string link = "", string type = "1")
        {
            var db = new DataAccessDataContext();
            var token = db.userPushTbls.SingleOrDefault(c => c.userId == userID);
            if (token != null)
            {
                return SendPushNotification(new string[] { token.token }, body, title, link, type);
            }
            return "user-not-registered";
        }
        public static string SendToUser(long[] users, string body, string title, string link = "", string type = "1")
        {
            var db = new DataAccessDataContext();
            var tokens = db.userPushTbls.Where(c => users.Contains(c.userId));
            if (tokens.Any())
            {
                var ts = tokens.Select(c => c.token).ToArray<string>();
                return SendPushNotification(ts, body, title, link, type);
            }
            return "user-not-registered";
        }

        public static string SendToCustomer(long customerID, string body, string title, string link = "", string type = "1")
        {
            var db = new DataAccessDataContext();
            var tokens = db.customerPushTbls.Where(c => c.customerId == customerID);
            if (tokens.Any())
            {
                if (tokens.Any(c => c.isOneSignal))
                {
                    var ts = tokens.Where(c => c.isOneSignal == true).Select(c => c.token).ToArray();
                    return SendOneSignalPushNotification(ts, body, title, link, type);
                }
                else
                {
                    var ts = tokens.Select(c => c.token).ToArray<string>();
                    return SendPushNotification(ts, body, title, link, type);
                }
            }
            return "user-not-registered";
        }
        public static string SendToCustomer(long[] customers, string body, string title, string link = "", string type = "1")
        {
            var db = new DataAccessDataContext();
            var tokens = db.customerPushTbls.Where(c => customers.Contains(c.customerId));
            if (tokens.Any())
            {
                if (tokens.Any(c => c.isOneSignal))
                {
                    var ts = tokens.Where(c => c.isOneSignal == true).Select(c => c.token).ToArray();
                    return SendOneSignalPushNotification(ts, body, title, link, type);
                }
                else
                {
                    var ts = tokens.Select(c => c.token).ToArray<string>();
                    return SendPushNotification(ts, body, title, link, type);
                }
            }
            return "user-not-registered";
        }

    }
}