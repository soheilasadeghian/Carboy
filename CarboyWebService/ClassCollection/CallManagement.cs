using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace CarBoyWebservice.ClassCollection
{
    public static class CallManagement
    {
        public static void SendToFileShare(string mobile,string fileName, params char[] numbers)
        {

            //StringBuilder number = new StringBuilder();
            //foreach (var item in numbers)
            //{
            //    number.Append("CB" + item + "&");
            //}

            //string Data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/calls/"+fileName));
            //Data = Data.Replace("~", mobile).Replace("|", number.ToString());


            //string networkShareLocation = @"\\172.16.32.54\\carboy\\" + "s" + mobile + ".txt";

            //IntPtr token = IntPtr.Zero;
            //LogonUser("carboy", "carboy", "manoto@@",
            //    9, 0, ref token);
            //using (WindowsImpersonationContext person = new WindowsIdentity(token).Impersonate())
            //{

            //    File.WriteAllText(networkShareLocation, Data);

            //}

        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
       int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll")]
        private static extern Boolean CloseHandle(IntPtr hObject);
    }
}