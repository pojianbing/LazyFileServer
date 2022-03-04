using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server;
using Lazy.FileServer.Server.FilePathCalculators;
using Lazy.FileServer.Server.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileServerServiceCollectionExtensions
    {
        public static SimpleFileServerBuilder AddLazyFileServer(this IServiceCollection services, IConfiguration configuration, Action<FileServerOptions> optionsAction = default)
        {
            services.Configure<FileServerOptions>(configuration?.GetSection("FileServer"));
            if (optionsAction != null) services.PostConfigure<FileServerOptions>(optionsAction);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFilePathCalculator, DateFilePathCalculator>();
            services.AddScoped<IFilePathCalculator, HashFilePathCalculator>();
            services.AddScoped<IAppFinder, DefaultAppFinder>();
            services.AddScoped<IFileServer, DefaultFileServer>();

            return new SimpleFileServerBuilder(services);
        }
    }

    public class SimpleFileServerBuilder
    {
        private readonly IServiceCollection _services;

        public SimpleFileServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public SimpleFileServerBuilder AddFilePathCalculator<T>() where T : class, IFilePathCalculator
        {
            _services.AddScoped<IFilePathCalculator, T>();
            return this;
        }

        public SimpleFileServerBuilder ReplaceAppFinder<T>() where T : class, IAppFinder
        {
            _services.RemoveAll<IAppFinder>();
            _services.AddScoped<IAppFinder, T>();
            return this;
        }
    }
}