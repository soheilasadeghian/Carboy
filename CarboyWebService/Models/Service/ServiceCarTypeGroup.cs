using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.Service
{
    public class ProductGroup
    {
        public string name { get; set; }
        public List<ProductGroupDetail> productGroupDetail { get; set; }

    }
}