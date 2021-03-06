﻿// Copyright (c) 2018. Licensed under the MIT License. See https://www.opensource.org/licenses/mit-license.php for full license information.

using System;
using System.IO;

using MentorBot.Functions.Abstract.Processor;
using MentorBot.Functions.Abstract.Services;
using MentorBot.Functions.Connectors;
using MentorBot.Functions.Models.Options;
using MentorBot.Functions.Processors;
using MentorBot.Functions.Services;
using MentorBot.Localize;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MentorBot.Functions.App
{
#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>Service locator is normally bad practice, but other methods are not realiable in Azure Functions.</summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>Configure the service provider if not configured</summary>
        public static void EnsureServiceProvider()
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = BuildServiceProvider();
            }
        }

        /// <summary>Get a service.</summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        public static T Get<T>() => _serviceProvider.GetService<T>();

        private static IServiceProvider BuildServiceProvider()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", true, false)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            services.AddTransient<IAsyncResponder, HangoutsChatConnector>();
            services.AddTransient<IHangoutsChatService, HangoutsChatService>();
            services.AddTransient<ICognitiveService, CognitiveService>();
            services.AddTransient<ICommandProcessor, LocalTimeProcessor>();
            services.AddTransient<ICommandProcessor, RepeatProcessor>();
            services.AddTransient<IStringLocalizer, StringLocalizer>();
            services.AddSingleton(new GoogleCloudOptions(config));
            services.AddSingleton<IDocumentClientService>(
                new DocumentClientService(config["AzureCosmosDBAccountEndpoint"], config["AzureCosmosDBKey"]));

            return services.BuildServiceProvider(false);
        }
    }
#pragma warning restore S120
}
