using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models
{
    public class TrackerModel
    {
        public long id { get; set; }
        public string fullName { get; set; }
        public string tel { get; set; }
        public string sp { get; set; }
        public string image { get; set; }
        public string hash { get; set; }
        public string srclat { get; set; }
        public string srclng { get; set; }
        public string deslat { get; set; }
        public string deslng { get; set; }
        
    }
}