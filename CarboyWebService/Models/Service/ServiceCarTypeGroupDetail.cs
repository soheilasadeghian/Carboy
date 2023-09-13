using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.Service
{
    public class ProductGroupDetail
    {
        public long ID { get; set; }
        public string name { get; set; }
        public bool isOffer { get; set; }
        public string offerText { get; set; }
        public double price { get; set; }
        public double priceWithDiscount { get; set; }

    }
}