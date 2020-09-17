using CCPP.Core.Queries;
using CCPP.Core.Repository;
using CCPP.Data.Queries;
using CCPP.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCPP.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CcppDbContext>(o => o.UseSqlServer(config.GetConnectionString("CcppDb")));
            services.AddScoped<IPaymentTranstactionsRepository, PaymentTranstactionsRepository>();

            services.AddScoped<IPaymentTranstactionQueries, PaymentTransactionQueries>();
            return services;
        }
    }
}
