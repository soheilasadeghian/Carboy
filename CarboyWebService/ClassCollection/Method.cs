using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CarBoyWebservice.ClassCollection
{
    public static class Method
    {
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        public static string Url { get { return ConfigurationManager.AppSettings["url"]; } }
        public static string UploadPath { get { return ConfigurationManager.AppSettings["upload-path"]; } }
        public static string UploadUrl { get { return ConfigurationManager.AppSettings["upload-url"]; } }
        public static bool IsPointInPolygon(PointF point, PointF[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.X, pointY = point.Y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            PointF endPoint = polygon[polygonLength - 1];
            endX = endPoint.X;
            endY = endPoint.Y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.X; endY = endPoint.Y;
                //
                inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static string md5(string sPassword)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
        public static bool checkPlateNumber(string plateNumber)
        {
            var parts = plateNumber.Split('-');

            if (parts.Count() != 4)
                return false;

            int number;

            if (int.TryParse(parts[0], out number) == false)
                return false;

            if (int.TryParse(parts[2], out number) == false)
                return false;

            if (int.TryParse(parts[3], out number) == false)
                return false;

            if (parts[0].Length != 2)
                return false;

            if (parts[2].Length != 3)
                return false;

            if (parts[3].Length != 2)
                return false;

            var list = new List<string> {
                "الف","ب","پ","ت","ث","ج",
                "چ","ح","خ","د","ذ","ر",
                "ز","ژ","س","ش","ص","ض",
                "ط","ظ","ع","غ","ف","ق",
                "ک","گ","ل","م","ن","و","ه"
                ,"ی"
            };

            if (!list.Any(c => c == parts[1]))
                return false;

            return true;
        }
        public static string convertPersianNumberToEnglishNumber(string str)
        {
            char[] temp = str.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                switch (Convert.ToInt32(temp[i]).ToString())
                {
                    case "1632":
                        temp[i] = '0';
                        break;
                    case "1633":
                        temp[i] = '1';
                        break;
                    case "1634":
                        temp[i] = '2';
                        break;
                    case "1635":
                        temp[i] = '3';
                        break;
                    case "1636":
                        temp[i] = '4';
                        break;
                    case "1637":
                        temp[i] = '5';
                        break;
                    case "1638":
                        temp[i] = '6';
                        break;
                    case "1639":
                        temp[i] = '7';
                        break;
                    case "1640":
                        temp[i] = '8';
                        break;
                    case "1641":
                        temp[i] = '9';
                        break;

                    case "1776":
                        temp[i] = '0';
                        break;
                    case "1777":
                        temp[i] = '1';
                        break;
                    case "1778":
                        temp[i] = '2';
                        break;
                    case "1779":
                        temp[i] = '3';
                        break;
                    case "1780":
                        temp[i] = '4';
                        break;
                    case "1781":
                        temp[i] = '5';
                        break;
                    case "1782":
                        temp[i] = '6';
                        break;
                    case "1783":
                        temp[i] = '7';
                        break;
                    case "1784":
                        temp[i] = '8';
                        break;
                    case "1785":
                        temp[i] = '9';
                        break;
                    default:

                        break;

                }
            }

            return new string(temp);


            // return value.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
        }
        public static Bitmap generatePlate(string plateNumber)
        {
            var parts = plateNumber.Split('-');

            return PlateGenerator.Engine.Generate(parts[0], parts[1], parts[2], parts[3]);

        }
        public static string RandomNumber()
        {
            int maxSize = 6;// تعداد رقم های کد رهگیری
            char[] chars = new char[10];
            string a;
            a = "1234567890";//در اینجا می توانیم درصورت نیاز حروف بزرگ را هم اضافه کنیم
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }//تولید کد 6 رقمی

        #region persianFormatDate

        /// <summary>
        /// "۱۵ مهر ۹۶"
        /// </summary>
        public static string persianFormatDate_m(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("m");
            return newDateTime;
        }

        /// <summary>
        /// "۱۳۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_h(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("h");
            return newDateTime;
        }

        /// <summary>
        /// "۱۳۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_D(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("D");
            return newDateTime;
        }

        /// <summary>
        /// "۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_d(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("d");
            return newDateTime;
        }

        /// <summary>
        /// "شنبه ۱۵ مهر ۹۶"
        /// </summary>
        public static string persianFormatDate_n(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("n");
            return newDateTime;
        }

        /// <summary>
        /// "۱۵:۱۳:۱۰"
        /// </summary>
        public static string persianFormatDate_H(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("H");
            return newDateTime;
        }

        /// <summary>
        /// "1396/07/15"
        /// </summary>
        public static string persianFormatDate_non(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString();
            return newDateTime;
        }

        /// <summary>
        /// "۱۵ مهر ۱۳۹۶"
        /// </summary>
        public static string persianFormatDate_M(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToString("M");
            return newDateTime;
        }

        /// <summary>
        /// "۱۳۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_Re_H(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToRelativeDateString("H");
            return newDateTime;
        }

        /// <summary>
        /// "۱۳۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_Re_M(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToRelativeDateString("M");
            return newDateTime;
        }

        /// <summary>
        /// "۱۳۹۶/۷/۱۵"
        /// </summary>
        public static string persianFormatDate_Re_TY(DateTime dateTime)
        {
            string newDateTime = "";
            newDateTime = Persia.Calendar.ConvertToPersian(dateTime).ToRelativeDateString("TY");
            return newDateTime;
        }
        #endregion

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
        public static long ToUnixTime(DateTime date)
        {
            return Convert.ToInt64((date - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }
        public static string RNGCharacterMask()
        {
            int maxSize = 6;// تعداد رقم های کد رهگیری
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyz1234567890";//در اینجا می توانیم درصورت نیاز حروف بزرگ را هم اضافه کنیم
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }//تولید کد سفارش


        public static Bitmap getMap(List<GoogleMapsApi.Entities.Common.Location> locations, int width)
        {

            IList<GoogleMapsApi.StaticMaps.Entities.Path> finalPath = new List<GoogleMapsApi.StaticMaps.Entities.Path>();
            IList<ILocationString> markers = new List<ILocationString>();

            int size = locations.Count;
            Location lastPoint = null;

            for (int i = 0; i < size - 1; i += 1)
            {
                IList<ILocationString> innerPath = new List<ILocationString>();

                Location Origin = locations[i];
                Location Destination = locations[i + 1];
                lastPoint = Destination;

                var DirectionRequest = new DirectionsRequest();
                DirectionRequest.ApiKey = "AIzaSyAw7DIrQdashMVoy6KrbVRla97e_DoOzYE";

                DirectionRequest.Origin = Origin.LocationString;
                DirectionRequest.Destination = Destination.LocationString;

                var result = GoogleMapsApi.GoogleMaps.Directions.Query(DirectionRequest);

                var steps = result.Routes.First().Legs.First().Steps.Select(step => new Location(step.StartLocation.Latitude, step.StartLocation.Longitude)).ToList<ILocationString>();

                innerPath.Add(Origin);
                markers.Add(Origin);

                for (int j = 1; j < steps.Count() ; j++)
                {
                    var step = steps[j];
                    innerPath.Add(step);
                }

                var pp = new GoogleMapsApi.StaticMaps.Entities.Path()
                {
                    Locations = innerPath,
                    Style = new PathStyle
                    {
                        Color = ColorHandler.getInstance().NextColor
                    }
                };

                finalPath.Add(pp);

                // finalPath.Add(Destination);
            }
            markers.Add(locations.Last());

            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

            IList<Marker> ms = new List<Marker>();
            ms.Add(new Marker()
            {
                Locations = markers,
                Style = new MarkerStyle
                {
                    Color = "0x004370",
                    Size = GoogleMapsApi.StaticMaps.Enums.MarkerSize.Mid,
                }
            });

            string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(

             ms , new ImageSize(width, width))
            {
                Pathes = finalPath
            });

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            httpRequest.Timeout = 15000;

            httpRequest.ReadWriteTimeout = 20000;

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream imageStream = httpResponse.GetResponseStream();

            System.Drawing.Bitmap buddyIcon = new System.Drawing.Bitmap(imageStream);

            httpResponse.Close();

            imageStream.Close();

            return buddyIcon;

        }


    }
}