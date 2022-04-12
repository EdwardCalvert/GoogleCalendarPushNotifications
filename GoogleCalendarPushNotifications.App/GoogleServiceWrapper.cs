﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GoogleCalendarPushNotifications.App
{
    public class GoogleServiceWrapper
    {
        private static readonly HttpClient Client = new HttpClient();

        private CalendarService _calendarService;
        private ICredential _credential;
        private string _channelId;
        private string _resourceId;

        private string[] _scopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.Calendar };

        public GoogleServiceWrapper()
        { }

        /// <summary>
        /// Initialises the Google Calendar service.
        /// </summary>
        /// <returns>Success</returns>
        public async Task<bool> InitialiseService()
        {
            bool success = false;
            _credential = CreateCredential();

            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential
            });

            success = await TestAuthentication();
            return success;
        }

        /// <summary>
        /// Registers a new push notification channel with Google Calendar api.
        /// </summary>
        /// <returns>Success</returns>
        public async Task<bool> RegisterPushNotificationChannel()
        {
            bool success = false;
            _channelId = Guid.NewGuid().ToString();

            var values = new Dictionary<string, string>()
            {
                { "id", _channelId },
                { "type", "web_hook" },
                { "address", Constants.ReceivingUrl }
            };

            var response = await SendChannelRequest(values, Constants.GoogleChannelRegistrationUrl);
            success = response != null && response.IsSuccessStatusCode;

            if (success)
            {
                dynamic responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());
                _resourceId = responseBody.resourceId;
            }

            return success;
        }

        /// <summary>
        /// Deregisters an existing push notification channel
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeregisterPushNotificationChannel()
        {
            bool success = false;

            var values = new Dictionary<string, string>()
            {
                { "id", _channelId },
                { "resourceId", _resourceId }
            };

            var response = await SendChannelRequest(values, Constants.GoogleChannelDeregistrationUrl);
            success = response != null && response.IsSuccessStatusCode;

            return success;
        }

        /// <summary>
        /// Tests authentication with Google Calendar.
        /// </summary>
        private async Task<bool> TestAuthentication()
        {
            bool success = false;

            try
            {
                if (_calendarService != null)
                {
                    EventsResource.ListRequest req = _calendarService.Events.List(Constants.GoogleCalendarId);
                    req.TimeMin = DateTime.Now;
                    req.TimeMax = DateTime.Now.AddDays(1);
                    Events evs = await req.ExecuteAsync();
                    Console.WriteLine(evs.Items.Aggregate("", (sum, value) => sum = sum + "; " + value.Summary));

                    CalendarListResource.ListRequest request = _calendarService.CalendarList.List();
                    CalendarList calendar = await request.ExecuteAsync();

                    Console.WriteLine(calendar.Items.Aggregate("", (sum, value) => sum = sum + " " + value.Id + " " + value.Description));
                    success = true;
                }
            }
            catch (Exception)
            {
                // Do nothing.
            }

            return success;
        }

        /// <summary>
        /// Sends a channel request to the given url with the given values.
        /// </summary>
        /// <param name="values">Request body</param>
        /// <param name="channelUrl">Request destination</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendChannelRequest(Dictionary<string, string> values, string destination)
        {
            HttpResponseMessage response = null;

            try
            {
                string token = await _credential.GetAccessTokenForRequestAsync();
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(values);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await Client.PostAsync(destination, content);
            }
            catch (Exception)
            {
                // Do nothing.
            }

            return response;
        }

        /// <summary>
        /// Creates a service account credential object.
        /// </summary>
        /// <returns>ICredential</returns>
        private ICredential CreateCredential()
        {


            UserCredential credential;
            if (File.Exists(Constants.DesktopOauthClientIDFilePath))
            {
                using (var stream =
                    new FileStream(Constants.DesktopOauthClientIDFilePath, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        _scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }


                // Create Google Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "GoogleCallendar callback test",
                });


                return credential;
            }
            else
            {
                throw new FileNotFoundException("Please put the client secret file in the correct location");
            }
        }
    }
}
