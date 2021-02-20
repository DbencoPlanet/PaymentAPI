using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Core.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.Data.Contexts
{
    public class PaymentAPIDbContext :IdentityDbContext
    {
        public PaymentAPIDbContext(DbContextOptions<PaymentAPIDbContext> options)
          : base(options)
        {
        }
        public DbSet<PaymentCardModel> PaymentCardModels { get; set; }
    }
}
