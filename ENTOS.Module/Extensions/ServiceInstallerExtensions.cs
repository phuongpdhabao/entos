﻿using ENTOS.Module.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ENTOS.Module.Extensions
{
    public static class ServiceInstallerExtensions
    {
        /// <summary>
        /// Cài đặt các dịch vụ từ assembly hiện tại (nơi chứa mã nguồn này).
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InstallServicesFromAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            //var installers = Assembly.GetExecutingAssembly
            var installers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IServiceInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (var installer in installers)
            {
                installer.InstallServices(services, configuration);
            }

        }

        /// <summary>
        /// Cài đặt các dịch vụ từ tất cả các assembly đã nạp vào AppDomain hiện tại.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InstallServicesFromAssemblies(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic) // tránh lỗi assembly động
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IServiceInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (var installer in installers)
            {
                installer.InstallServices(services, configuration);
            }
        }

    }
}
