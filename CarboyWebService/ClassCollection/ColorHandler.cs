using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.ClassCollection
{
    public class ColorHandler
    {
        private static ColorHandler handler;

        private int index = 0;

        private Dictionary<int, string> dic = new Dictionary<int, string>() {
                { 0,"0xe3a21a" },
                { 1,"0xda532c" },
                { 2,"0x603cba" },
                { 3,"0x2d89ef" },
                { 4,"0x5d576b" },
                { 5,"0x00a300" },
                { 6,"0x9f00a7" },
                { 7,"0x03f7eb" },
                { 8,"0x5f1a37" },
                { 9,"0x772e25" }
            };

        public static ColorHandler getInstance()
        {
            if (handler == null)
            {
                handler = new ColorHandler();
            }

            return handler;
        }

        public string NextColor
        {
            get
            {
                string color =  dic[index++];
                if (index >= dic.Count)
                    index = 0;

                return color;
            }
        }
    }
}