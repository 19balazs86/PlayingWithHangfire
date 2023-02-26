# Playing with Hangfire
Easily perform background jobs in .NET applications using `Hangfire`.

This repository contains a Web API with default configurations and functionalities that can run in a console application alongside the [GenericHost as a WorkerService](https://github.com/19balazs86/PlayingWithGenericHost).

#### Resources

- [Hangfire](https://www.hangfire.io) 📓*Official page*
- [Perform Out-of-Process Tasks with retry and instrumentation for OpenTelemetry](https://wrapt.dev/blog/hangfire-helps-dotnet-perform-out-of-process-tasks) 📓*Wrapt - Paul DeVito* 
- [Using Hangfire with ASP.NET](https://damienbod.com/2023/02/20/using-hangfire-with-asp-net-core) 📓*Damien Bod* 
- [Hangfire in .NET 6 - Background jobs](https://www.c-sharpcorner.com/article/hangfire-in-net-core-6-background-jobs) 📓*C# Corner* 
- [Securing dashboard with Custom Auth Policy](https://sahansera.dev/securing-hangfire-dashboard-with-endpoint-routing-auth-policy-aspnetcore) 📓*Sahan Serasinghe*

[Azure ServiceBus](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebussender.schedulemessageasync) also has a feature for scheduling messages