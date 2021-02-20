using Microsoft.Extensions.DependencyInjection;
using PaymentAPI.Application.ProcessPaymentApp;
using PaymentAPI.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddTransient<ICheapPaymentGatewayAppService, CheapPaymentGatewayAppService>();
            services.AddTransient<IExpensivePaymentGatewayAppService, ExpensivePaymentGatewayAppService>();
            services.AddTransient<IPremiumPaymentGatewayAppService, PremiumPaymentGatewayAppService>();
            return services;
        }
    }
}
