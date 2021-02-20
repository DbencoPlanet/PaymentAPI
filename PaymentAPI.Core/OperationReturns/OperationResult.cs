using Newtonsoft.Json;
using PaymentAPI.Core.Payment;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PaymentAPI.Core.OperationReturns
{
    public struct OperationResult
    {
        public bool Succeeded { get; set; }

        public OperationStatus Status { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public PaymentCardModel Payment { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
