namespace GoogleCalendarPushNotifications.App
{
    public class Constants
    {
        /// <summary>
        /// The Google account that owns the calendar we are registering for.
        /// </summary>
        public const string GoogleAccount = "[YOUR GSUITE EMAIL ADDRESS]";

        /// <summary>
        /// The calendar id of the Google calendar we are registering for.
        /// </summary>
        public const string GoogleCalendarId = "primary"; // Replace as neccessary- you can use primary for the primay calendar!

        /// <summary>
        /// JSON location, generated from  https://console.cloud.google.com/apis/credentials and of the desktop type. 
        /// </summary>
        public const string DesktopOauthClientIDFilePath = "[YOUR FILE NAME].apps.googleusercontent.com.json";

        /// <summary>
        /// The google url for registering notification channels.
        /// </summary>
        public const string GoogleChannelRegistrationUrl = "https://www.googleapis.com/calendar/v3/calendars/" + GoogleCalendarId + "/events/watch";

        /// <summary>
        /// The google url for deregistering notification channels.
        /// </summary>
        public const string GoogleChannelDeregistrationUrl = "https://www.googleapis.com/calendar/v3/channels/stop";

        /// <summary>
        /// The url to receive the google calendar push notifications.
        /// </summary>
        public const string ReceivingUrl = "[Your URL]/api/googlenotifications/events";

        public const string DeregisterChannel = "Press any key to deregister channel...";
        public const string TerminateProgram = "Press any key to terminate...";
        public const string GoogleAuthenticationSuccess = "Authentication with Google Calendar api was successful...";
        public const string GoogleAuthenticationFailure = "FAIL: Authentication with Google Calendar api failed...";
        public const string RegisterChannelSuccess = "Registering push notification channel with Google Calendar api was successful...";
        public const string RegisterChannelFailure = "FAIL: Registering push notification channel with Google Calendar api failed...";
        public const string DeregisterChannelSuccess = "Deregistering push notification channel with Google Calendar api was successful...";
        public const string DeregisterChannelFailure = "FAIL: Deregistering push notification channel with Google Calendar api failed...";
    }
}
