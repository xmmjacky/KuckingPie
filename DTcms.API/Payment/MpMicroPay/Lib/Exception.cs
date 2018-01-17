using System;
using System.Collections.Generic;
using System.Web;

namespace DTcms.API.Payment.MpMicroPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}