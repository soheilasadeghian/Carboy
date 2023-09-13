using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.Models.Service
{
    public class ServiceOrder
    {
        public long repairmanId { get; set; }
        public DateTime requestTime { get; set; }
        public long customerId { get; set; }
        public long packageId { get; set; }
        public int timePeriod { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public byte state { get; set; }
        public DateTime regDate { get; set; }

        public string carmanName { get; set; }
        public string carmanFamily { get; set; }
        public string carmanMobile  { get; set; }

        public string distributorName { get; set; }
        public string distributorFamily { get; set; }
        public string distributorMobile { get; set; }

        public string customerName { get; set; }
        public string customerFamily { get; set; }
        public string customerMobile { get; set; }

        public string servicePriceTariffDayName { get; set; }
        public double servicePriceTariffPercent { get; set; }
        public DateTime servicePriceTariffStartTime { get; set; }
        public DateTime servicePriceTariffEndTime { get; set; }
        
    }
}