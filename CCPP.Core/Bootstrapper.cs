using CCPP.Core.FileParsers;
using Microsoft.Extensions.DependencyInjection;

namespace CCPP.Core
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddSingleton<FileParser, XmlFileParser>();
            services.AddSingleton<FileParser, CsvFileParser>();
            return services;
        }
    }
}
