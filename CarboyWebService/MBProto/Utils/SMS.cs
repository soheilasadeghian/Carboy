using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace CarBoyWebservice.MBProto.Utils
{
    public class SMS
    {
        public static void SendSms(string to, string text)
        {

            new Thread(() =>
            {
                try
                {

                   smsService.smsSoapClient client = new smsService.smsSoapClient();
                    var result = client.doSendArraySMS("carw", "123@qwe",
                        new smsService.ArrayOfString() { "1000534834" },
                        new smsService.ArrayOfString() { to },
                        new smsService.ArrayOfString() { text },
                        new smsService.ArrayOfBoolean() { true },
                        new smsService.ArrayOfBoolean() { false },
                        new smsService.ArrayOfBoolean() { false },
                        new smsService.ArrayOfString() { "" });

                    result = result.Replace("Send OK.<ReturnIDs>", "").Replace("</ReturnIDs>", "");

                }
                catch
                {
                }
            }).Start();
        }

        public static string GenerateSmsCode()
        {
            string _numbers = "123456789";
            Random random = new Random(DateTime.Now.Millisecond);

            StringBuilder builder = new StringBuilder(5);
            string numberAsString = "";

            for (var i = 0; i < 5; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            numberAsString = builder.ToString();
            return numberAsString;

        }
    }
}