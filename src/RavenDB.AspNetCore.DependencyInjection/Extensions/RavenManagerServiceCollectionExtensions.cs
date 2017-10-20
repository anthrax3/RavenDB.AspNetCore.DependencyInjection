﻿using Microsoft.Extensions.DependencyInjection;
using RavenDB.AspNetCore.DependencyInjection.Options;
using System;
using Microsoft.Extensions.Configuration;

namespace RavenDB.AspNetCore.DependencyInjection
{
    /// <summary>
    /// Contains extension methods for <see cref="IServiceCollection"/> to configure the Raven services.
    /// </summary>
    public static class RavenManagerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default manager <see cref="RavenManager"/>.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="options">The options used to configure the Raven manager.</param>
        /// <returns>The <see cref="RavenBuilder"/> this method created.</returns>
        public static RavenBuilder AddRavenManager(
            this IServiceCollection services,
            Action<RavenManagerOptions> options)
        {
            return AddRavenManager<RavenManager, RavenManagerOptions>(services, options);
        }

        /// <summary>
        /// Adds and configures a default <see cref="RavenManager" />
        /// using options from the configuration root (typically a config file) that will
        /// bind to <see cref="RavenManagerOptions" />
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration root</param>
        /// <param name="section">Configuration section (key) to bind to options</param>
        public static RavenBuilder AddRavenManager(
            this IServiceCollection services,
            IConfigurationRoot configuration, string section = "Raven") {
            
            services.Configure<RavenManagerOptions>(configuration.GetSection(section));

            return AddRavenManager<RavenManager>(services);
        }

        /// <summary>
        /// Adds and configures the specified manager.
        /// </summary>
        /// <typeparam name="TValue">The type of the specified manager <see cref="IRavenManager"/>.</typeparam>
        /// <typeparam name="TOptions">The type of options needed to configure the specified manager.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="options">The options used to configure the manager.</param>
        /// <returns>The <see cref="RavenBuilder"/> this method created.</returns>
        public static RavenBuilder AddRavenManager<TValue, TOptions>(
               this IServiceCollection services,
               Action<TOptions> options = null)
                where TOptions : class
                where TValue : class, IRavenManager
        {
            if (options != null)
                services.Configure("Raven", options);
                
            return AddRavenManager<TValue>(services);
        }

        /// <summary>
        /// Adds the specified manager.
        /// </summary>
        /// <typeparam name="TValue">The type of the specified manager <see cref="IRavenManager"/>.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>The <see cref="RavenBuilder"/> this method created.</returns>
        public static RavenBuilder AddRavenManager<TValue>(
            this IServiceCollection services)
            where TValue : class, IRavenManager
        {
            services.AddSingleton<IRavenManager, TValue>();

            return new RavenBuilder(services);
        }
    }
}