using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.Core.OperationReturns
{
    public enum OperationStatus
    {
        ConcurrencyMismatch,
        Created,
        Deleted,
        Exists,
        NotFound,
        Unknown,
        Updated,
    }
}
