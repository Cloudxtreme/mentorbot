using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

using MentorBot.Functions.Models.Options;

namespace MentorBot.Functions.Connectors.Base
{
    /// <summary>The base class for all google connectors. This class provide helper methods like create new google service instance.</summary>
    /// <typeparam name="T">The type of the goolge service.</typeparam>
    public class GoogleBaseService<T>
        where T : BaseClientService
    {
        /// <summary>Initializes a new instance of the <see cref="GoogleBaseService{T}"/> class.</summary>
        protected GoogleBaseService(Lazy<T> serviceProviderFactory)
        {
            ServiceProviderFactory = serviceProviderFactory;
        }

        /// <summary>Gets the service provider factory.</summary>
        protected Lazy<T> ServiceProviderFactory { get; }

        /// <summary>Gets the service provide.</summary>
        protected T ServiceProvider => ServiceProviderFactory.Value;

        /// <summary>Creates the base client service initializer.</summary>
        public static BaseClientService.Initializer CreateInitializer(GoogleCloudOptions options, CreateInitializerTypes type, params string[] scopes)
        {
            switch (type)
            {
                case CreateInitializerTypes.ApiKey:
                    return new BaseClientService.Initializer
                    {
                        ApplicationName = options.GoogleCloudApplicationName,
                        ApiKey = options.GoogleCloudApiKey
                    };
                case CreateInitializerTypes.ServiceAccountStream:
                    return new BaseClientService.Initializer
                    {
                        ApplicationName = options.GoogleCloudApplicationName,
                        HttpClientInitializer = GoogleCredential.FromStream(options.GoogleCreadentialsStream).CreateScoped(scopes)
                    };
                default:
                    return null;
            }
        }
    }
}
