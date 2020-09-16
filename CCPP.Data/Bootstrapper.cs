using CCPP.Core.Repository;
using CCPP.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CCPP.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentTranstactionsRepository, PaymentTranstactionsRepository>();
            return services;
        }
    }
}
