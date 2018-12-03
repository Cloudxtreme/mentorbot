// Copyright (c) 2018. Licensed under the MIT License. See https://www.opensource.org/licenses/mit-license.php for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;

using Google;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

using MentorBot.Functions.Abstract.Connectors;
using MentorBot.Functions.Connectors.Base;
using MentorBot.Functions.Models.Options;

namespace MentorBot.Functions.Connectors
{
    /// <summary>Provider methods connected to google calendar service endpoints.</summary>
    /// <seealso cref="IGoogleCalendarConnector" />
    /// <seealso cref="GoogleBaseService{CalendarService}" />
    public class GoogleCalendarConnector : GoogleBaseService<CalendarService>, IGoogleCalendarConnector
    {
        private static readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.CalendarEventsReadonly };

        /// <summary>Initializes a new instance of the <see cref="GoogleCalendarConnector"/> class.</summary>
        public GoogleCalendarConnector(GoogleCloudOptions options)
            : this(new Lazy<CalendarService>(
                () => new CalendarService(CreateInitializer(options, CreateInitializerTypes.ServiceAccountStream, Scopes))))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GoogleCalendarConnector"/> class.</summary>
        public GoogleCalendarConnector(Lazy<CalendarService> serviceProviderFactory)
            : base(serviceProviderFactory)
        {
        }

        /// <inheritdoc/>
        public async Task<Event> GetNextMeetingAsync(string calendarId)
        {
            var now = DateTime.Now;

            var request = ServiceProvider.Events.List(calendarId);

            request.MaxResults = 1;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.TimeMin = now;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            request.TimeMax = now.AddDays(1);

            try
            {
                var response = await ExecuteAsync(request).ConfigureAwait(false);
                return response?.Items?.FirstOrDefault();
            }
            catch (GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        /// <summary>Execute asyncronius request.</summary>
        /// <typeparam name="T">The type of the request result.</typeparam>
        protected virtual Task<T> ExecuteAsync<T>(CalendarBaseServiceRequest<T> request)
            where T : class
        {
            return request.ExecuteAsync();
        }
    }
}
