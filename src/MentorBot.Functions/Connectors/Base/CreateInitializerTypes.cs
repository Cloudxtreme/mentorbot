// Copyright (c) 2018. Licensed under the MIT License. See https://www.opensource.org/licenses/mit-license.php for full license information.

namespace MentorBot.Functions.Connectors.Base
{
    /// <summary>Enumerable defining methods to create Google service initializer.</summary>
    public enum CreateInitializerTypes : byte
    {
        /// <summary>Create using the Google API key.</summary>
        ApiKey = 1,

        /// <summary>Create using the service account certificate stream.</summary>
        ServiceAccountStream = 2
    }
}
