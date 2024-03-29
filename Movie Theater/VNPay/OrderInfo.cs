﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Theater.VNPay
{
    public class OrderInfo
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public int Quantity { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}