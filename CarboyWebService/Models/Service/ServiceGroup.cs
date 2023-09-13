using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.Service
{
    public class ServiceGroup
    {
        public long ID { get; set; }
        public bool isRequired { get; set; }
        public bool isMultiSelect { get; set; }
        public int priority { get; set; }
        public List<Service> service { get; set; }

    }
}