using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using System.Runtime.InteropServices;
using FirebaseAdmin.Messaging;

namespace NotificationFuncation
{
    public static class NotificationFuncation
    {
        static NotificationFuncation()
        {
            // Initialize Firebase only once
            FirebaseApp.Create(new AppOptions()
            {   
                Credential = GoogleCredential.FromFile("path/to/your/firebase_credentials.json")
            });
            //Credential = GoogleCredential.FromFile("path/to/your/firebase_credentials.json")
        }

        [FunctionName("SendNotification")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string token = data?.token;
            string title = data?.title;
            string body = data?.body;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body))
            {
                return new BadRequestObjectResult("Please provide a device token, title, and body for the notification.");
            }

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Token = token
            };
            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return new OkObjectResult($"Notification sent successfully: {response}");
            }
            catch (FirebaseMessagingException ex)
            {
                log.LogError($"Error sending notification: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);
        }
    }
}
