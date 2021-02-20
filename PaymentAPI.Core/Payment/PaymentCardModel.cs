using PaymentAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PaymentAPI.Core.Payment
{
    public class PaymentCardModel
    {
        public long Id { get; set; }

        [Display(Name = "Credit Card Number")]
        [Required(ErrorMessage = "required")]
        [MaxLength(16)]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public string CardHolder { get; set; }

        public DateTime ExpirationDate { get; set; }

        [MaxLength(3)]
        public string SecurityCode { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }


    }
}
