using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.UserMessage
{
    public class Message
    {
        public long ID { get; set; }
        public string text { get; set; }
        public bool isRead { get; set; }
        public int type { get; set; }
        public string regDate { get; set; }
    }
}