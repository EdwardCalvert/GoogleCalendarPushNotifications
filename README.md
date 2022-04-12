# GoogleCalendarPushNotifications

# TLDR: [Get started](#get-started )

## ⚠Caution: The doccumentation is poor. To avoid disappointment, ensure you are familiar with Hosting .Net Core & have experience in Google Calendar. 

Test app setup to act as an end to end proof of concept for Google Calendar API push notifications.

Read the rubbish guide at: https://developers.google.com/calendar/api/guides/push. However, it does consider some important design points:
- More than one subscription could be valid at any point
- The recieving URL will need HTTPS with a valid certificate. 

## .App
Console application that allows you to subscribe to the events changing. 

![image](https://user-images.githubusercontent.com/72658447/163015416-1a706ff2-e57b-492f-a0f6-a165bae72203.png)

## .Web

A basic .net API controller that Google can post to. Any post request will be written to the console.  

![image](https://user-images.githubusercontent.com/72658447/163016072-0b4108e4-c886-4baa-88ae-4fd458129758.png)

## Get started 
### Desktop OAuth Credentials
Create a set of OAuth Desktop credentials [from the developer console](https://console.cloud.google.com/apis/credentials). 

Generate the correct credential type. 

![image](https://user-images.githubusercontent.com/72658447/163017896-f6d14680-a3c8-4844-998e-622edd1c74d9.png)

When asked, choose Desktop application, as this will callback to ``` localhost ``` without any configuration. 
![image](https://user-images.githubusercontent.com/72658447/163017936-0fec901e-f3fa-46cd-9ef5-1440e6013445.png)

Save the .json file locally, and add to your project in a suitable location wherever you wish. 

### You will need a way of Google getting to you. 

#### Reverse proxy
Use any reverse proxy of your choice, but you will need a HTTPS certificate. 


#### Ngrok
The free version of Ngrok allows you to create HTTPS tunnels to your development machine- perfect!

Follow the download instructions [https://ngrok.com/](https://ngrok.com/), and run in a powershell window
```powershell
.\ngrok http <YOUR PORT>
```

Keep this window open, and make a copy of the HTTPS address. 
![image](https://user-images.githubusercontent.com/72658447/163016946-d53d5399-621d-4bfe-b534-4bf453222da2.png)

### Fill in the constants page. 
The [.App](https://github.com/EdwardCalvert/GoogleCalendarPushNotifications/blob/master/GoogleCalendarPushNotifications.App/Constants.cs) has a page of constansts you need to fill out. 




```C# 
        public const string GoogleAccount = "[YOUR GSUITE EMAIL ADDRESS]";

        public const string GoogleCalendarId = "primary"; // Replace as neccessary- you can use primary for the primay calendar!

        public const string DesktopOauthClientIDFilePath = "[YOUR FILE NAME].apps.googleusercontent.com.json";

        public const string GoogleChannelRegistrationUrl = "https://www.googleapis.com/calendar/v3/calendars/" + GoogleCalendarId + "/events/watch";
        
        public const string GoogleChannelDeregistrationUrl = "https://www.googleapis.com/calendar/v3/channels/stop";
        
       public const string ReceivingUrl = "[Your URL]/api/googlenotifications/events";
```


## Now run the thing ...

Since you will have the .Web and .App to run, I suggest locating a PowerShell window, and from within the  ``` GoogleCalendarPushNotifications.Web ``` run the ``` dotnet run``` command. 

Then run the .App from Visual Studio.


## Enhancements

In the near future, I aim to add a fully Server Side implementation. 

Contributions warmly accepted. 
