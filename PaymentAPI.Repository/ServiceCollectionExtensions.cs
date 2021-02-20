using Microsoft.Extensions.DependencyInjection;
using PaymentAPI.Repository.ProcessPayment;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<ICheapPaymentGatewayRepository, CheapPaymentGatewayRepository>();
            services.AddTransient<IExpensivePaymentGatewayRepository, ExpensivePaymentGatewayRepository>();
            services.AddTransient<IPremiumPaymentGatewayRepository, PremiumPaymentGatewayRepository>();
            return services;
        }
    }
}
