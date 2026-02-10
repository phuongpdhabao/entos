using System;
using DevExpress.ExpressApp;
using Microsoft.Extensions.DependencyInjection;

namespace ENTOS.Module.Helpers
{


    public static class DependencyInjectionHelper
    {
        /// <summary>
        /// Lấy service theo kiểu T.
        /// </summary>
        public static T GetService<T>(XafApplication xafApplication) where T : class
        {
            return xafApplication.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Lấy service theo kiểu T (bắt buộc tồn tại).
        /// </summary>
        public static T GetRequiredService<T>(XafApplication xafApplication) where T : class
        {
            return xafApplication.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Lấy service theo kiểu runtime.
        /// </summary>
        public static object GetService(XafApplication xafApplication, Type type)
        {
            return xafApplication.ServiceProvider.GetService(type);
        }

        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Gán IServiceProvider từ Startup hoặc Program.
        /// </summary>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Lấy service theo kiểu T.
        /// </summary>
        public static T GetService<T>() where T : class
        {
            EnsureConfigured();
            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Lấy service theo kiểu T (bắt buộc tồn tại).
        /// </summary>
        public static T GetRequiredService<T>() where T : class
        {
            EnsureConfigured();
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Lấy service theo kiểu runtime.
        /// </summary>
        public static object GetService(Type type)
        {
            EnsureConfigured();
            return _serviceProvider.GetService(type);
        }

        private static void EnsureConfigured()
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("DependencyInjectionHelper is not configured. Call Configure(...) first.");
        }
    }

}
