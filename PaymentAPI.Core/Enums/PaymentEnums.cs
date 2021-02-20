using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentAPI.Core.Enums
{
    public enum PaymentStatus
    {
        [Description("Pending")]
        [Display(Name = "Pending")]
        Pending = 1,

        [Description("Processed")]
        [Display(Name = "Processed")]
        Processed = 2,

        [Description("Failed")]
        [Display(Name = "Failed")]
        Failed = 3
    }
}
