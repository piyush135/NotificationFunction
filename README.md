# Azure Notification Function

This Azure Function is designed to send push notifications to users using Firebase Cloud Messaging (FCM). The function is triggered by HTTP requests and can be called by other services to deliver notifications to mobile or web clients.

## Prerequisites

- **Azure Account**: Ensure you have an Azure account to deploy and manage the function.
- **Firebase Project**: You need an active Firebase project with Firebase Cloud Messaging enabled. Retrieve the `Server Key` from Firebase to authorize FCM requests.
- **Visual Studio 2022**: This project is built using .NET and requires Visual Studio 2022 for local development and deployment.

## Project Structure

The main files in this project are:

- `NotificationFunction.cs`: Contains the main function logic to process requests and send notifications.
- `local.settings.json`: Stores local configuration, such as Firebase API keys and other environment variables, for local development.
- `host.json`: Contains function app settings and configuration.
- `README.md`: Documentation for the function.

## Configuration

1. **Firebase API Key**:
   - Retrieve your Firebase server key from your Firebase project (found under `Project Settings > Cloud Messaging > Server key`).
   - Add the key to your `local.settings.json` file.

2. **local.settings.json**: This file holds environment variables for local development. Ensure this file includes the following keys:

    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "<Your_Azure_Storage_Connection_String>",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "FirebaseServerKey": "<Your_Firebase_Server_Key>"
      }
    }
    ```

3. **host.json**: You may configure additional settings such as logging and timeout behavior.

## Function Code Overview

The function is an HTTP-triggered function that takes an HTTP request with a JSON payload containing notification details.

### Sample Payload

```json
{
  "title": "Hello User",
  "message": "This is a sample notification.",
  "deviceToken": "user_device_token_here"
}
