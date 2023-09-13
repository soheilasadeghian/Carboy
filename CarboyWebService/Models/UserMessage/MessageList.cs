using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.UserMessage
{
    public class MessageList
    {
        public List<Message> message { get; set; }
        public int pageIndex { get; set; }
        public int totalCount { get; set; }
    }
}