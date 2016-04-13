# Stormpath Middleware for ASP.NET Core
This library makes it incredibly simple to add user authentication to your application, such as login, signup, authorization, and social login.

[Stormpath](https://stormpath.com) is a User Management API that reduces development time with instant-on, scalable user infrastructure. Stormpath's intuitive API and expert support make it easy for developers to authenticate, manage and secure users and roles in any application.

## Working Example

Head over to the [stormpath-aspnetcore-example](https://github.com/stormpath/stormpath-aspnetcore-example) repository to see a working example of ASP.NET MVC6 + Stormpath in action. :+1:

## Other Frameworks?

We're working on support for [ASP.NET 4.x](https://github.com/stormpath/stormpath-dotnet-owin-middleware/issues/4) and [Nancy](https://github.com/stormpath/stormpath-dotnet-owin-middleware/issues/5) right now. If you'd like to be notified when those packages are released, subscribe to the linked issues or send an email to support@stormpath.com. In the meantime, refer to the [generic OWIN quickstart](https://github.com/stormpath/stormpath-dotnet-owin-middleware/blob/master/README.md#quickstart) to add Stormpath to other frameworks.

## Quickstart

You can add Stormpath to a new or existing ASP.NET Core project with only two lines of code! Here's how:

1. **[Sign up](https://api.stormpath.com/register) for Stormpath**

2. **Get Your Key File**

  [Download your key file](https://support.stormpath.com/hc/en-us/articles/203697276-Where-do-I-find-my-API-key-) from the Stormpath Console.

3. **Store Your Key as Environment Variables**

  Open your key file and grab the **API Key ID** and **API Key Secret**, then run these commands in PowerShell (or the Windows Command Prompt) to save them as environment variables:

  ```
  setx STORMPATH_CLIENT_APIKEY_ID "[value-from-properties-file]"
  setx STORMPATH_CLIENT_APIKEY_SECRET "[value-from-properties-file]"
  ```

4. **Store Your Stormpath Application Href in an Environment Variable**

  Grab the `href` (called **REST URL** in the Stormpath Console UI) of your Application. It should look something like this:

  `https://api.stormpath.com/v1/applications/q42unYAj6PDLxth9xKXdL`

  Save this as an environment variable:

  ```
  setx STORMPATH_APPLICATION_HREF "[your Application href]"
  ```
  
  > :pushpin: It's also possible to specify the Application href at runtime by passing a configuration object when you initialize the middleware.

5. **Create an Application**

 Skip this step if you are adding Stormpath to an existing application.
 
 Use the Web - ASP.NET Web Application - ASP.NET 5 Web Application template in Visual Studio, with Authentication set to No Authentication.
 
 Alternatively, use [`yo aspnet`](https://github.com/OmniSharp/generator-aspnet) to scaffold a new project with a couple of keystrokes!
 
6. **Install the Middleware**

 Edit your `projecct.json` file, or use the Package Manager Console:
 
 ```
 PM> install-package Stormpath.AspNetCore
 ```
 
7. **Configure and Add the Middleware**

 First, add Stormpath to your services collection in `ConfigureServices`:
 
 ```csharp
 services.AddStormpath();
 ```
 
 The `AddStormpath` method takes an optional configuration object. If you want to hardcode the Application href, instead of storing it in environment variables, for example:
 
 ```csharp
 var myConfiguration = new StormpathConfiguration
 {
     Application = new ApplicationConfiguration
     {
         Name = "My Application"
     }
 }
 ```
 
 Once you've added Stormpath to the services collection, add it to your pipeline in `Configure`:
 
 ```csharp
 app.UseStormpath();
 ```
 
 Make sure you add Stormpath **before** other middleware such as MVC.
 
8. **That's It!**

  Compile and run your project, and use a browser to access `/login`. You should see a login view. MVC and Web API routes can be protected by adding `[Authorize]` attributes to the appropriate controller or method.


## Getting Help
If you encounter problems while using this library, file an issue here on Github or reach out to support@stormpath.com.
