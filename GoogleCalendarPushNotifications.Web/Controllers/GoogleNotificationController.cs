using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GoogleCalendarPushNotifications.Web.Controllers
{
    [Route("api/googlenotifications/")]
    public class GoogleNotificationController : Controller
    {
        public GoogleNotificationController()
        { }

        /// <summary>
        /// Receiving endpoint for Google Calendar push notifications.
        /// </summary>
        /// <returns>IActionResult</returns>
        ///<example>User-Agent = APIs-Google; (+https://developers.google.com/webmasters/APIs-Google.html)
        /// Content-Length = 0
        ///X-Forwarded-For = 74.125.212.14
        ///X-Forwarded-Proto = https
        ///X-Goog-Channel-Expiration = Tue, 19 Apr 2022 15:55:48 GMT
        ///X-Goog-Channel-Id = ff5d6214-116f-47a2-8ffa-0c3ee079411c
        ///X-Goog-Message-Number = 1302465
        ///X-Goog-Resource-Id = Kiip7BwJNukmPXTOFwPRWyPf0lY
        ///X-Goog-Resource-State = exists
        ///X-Goog-Resource-Uri = https://www.googleapis.com/calendar/v3/calendars/primary/events?alt=json</example> 
        ///
        [HttpPost("events")]
        public async Task<IActionResult> Post()
        {

            foreach (string key in HttpContext.Request.Headers.Keys)
            {
                Console.WriteLine(key + " = "
                    + HttpContext.Request.Headers[key]);
            }
            Console.WriteLine($"Resource ID Changed:  {HttpContext.Request.Headers["X-Goog-Resource-Id"]}");


            // Rewind, so the core is not

            return new OkResult();
        }

        [HttpGet("bob")]
        public string Index()
        {

            return "Bob";
        }
    }
}
