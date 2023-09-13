using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.Service
{
    public class Service
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public bool isSubService { get; set; }
        public double price { get; set; }//قیمت اصلی سرویس
        public double taxPrice { get; set; }
        public int priority { get; set; }
        public double serviceDiscountValue { get; set; }//مبلغ یا درصد تخفیف
        public double servicePriceWithDiscount { get; set; }//قیمت با اعمال تخفیف  
        

    }
}