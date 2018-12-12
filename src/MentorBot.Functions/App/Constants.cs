﻿// Copyright (c) 2018. Licensed under the MIT License. See https://www.opensource.org/licenses/mit-license.php for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MentorBot.Functions
{
    /// <summary>Contains all global application constant.</summary>
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        /// <summary>The name of the google service account file.</summary>
        public const string GoogleServiceAccountFileName = "mentorbotlearningcenter/certs/google-service-account.json";

        /// <summary>Gets the swagger json endpoint.</summary>
        public static string SwaggerJsonEndpoint => "/swagger/v1/swagger.json";
    }
}
