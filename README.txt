# Brainy.Mediator

A lightweight, dependency-injection friendly Mediator library for .NET.

Brainy.Mediator is an open-source implementation of the Mediator pattern designed to simplify communication between different parts of your application. It helps you write clean, maintainable, and loosely coupled code without introducing unnecessary complexity.

Originally developed by **Brainy Solutions** for use in enterprise applications, this library has been refined and released as a free, open-source project to support the .NET community.

---

## Why Brainy.Mediator?

As our projects grew, we wanted a mediator that was simple, lightweight, easy to understand, and easy to customize. While there are excellent mediator libraries available, we wanted complete control over the implementation and the flexibility to extend it for our own architecture.

Rather than keeping it private, we decided to share it with the community in the hope that it helps other developers build cleaner applications.

---

## Features

- Request / Response messaging
- Command and Query support
- Notification publishing
- Multiple notification handlers
- Pipeline Behaviors
- Dependency Injection integration
- Asynchronous processing
- Generic request handlers
- Lightweight implementation
- No unnecessary dependencies
- Free and Open Source

---

## Installation

Install from NuGet:

```bash
dotnet add package Brainy.Mediator
```

or

```powershell
Install-Package Brainy.Mediator
```

---

## Registering the Mediator

```csharp
builder.Services.AddScoped<IMediator, BrainyMediator>();
```

Register your handlers as usual.

```csharp
builder.Services.AddScoped<IRequestHandler<CreateStudentCommand, long>, CreateStudentCommandHandler>();

builder.Services.AddScoped<IRequestHandler<UpdateStudentCommand, int>, UpdateStudentCommandHandler>();
```

---

## Sending Requests

Define a request.

```csharp
public sealed record CreateStudentCommand(
    string Name,
    string Address
) : IRequest<long>;
```

Create its handler.

```csharp
public class CreateStudentCommandHandler
    : IRequestHandler<CreateStudentCommand, long>
{
    public async Task<long> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        // business logic

        return 1;
    }
}
```

Send the request.

```csharp
var id = await mediator.SendAsync(
    new CreateStudentCommand("Ali", "Islamabad"));
```

---

## Notifications

Create a notification.

```csharp
public class StudentCreatedNotification : INotification
{
    public long StudentId { get; set; }
}
```

Create one or more handlers.

```csharp
public class EmailNotificationHandler
    : INotificationHandler<StudentCreatedNotification>
{
    public async Task HandleAsync(
        StudentCreatedNotification notification,
        CancellationToken cancellationToken)
    {
    }
}
```

Publish the notification.

```csharp
await mediator.PublishAsync(
    new StudentCreatedNotification
    {
        StudentId = id
    });
```

Every registered notification handler will execute.

---

## Pipeline Behaviors

Pipeline behaviors allow code to run before and after every request.

Typical use cases include:

- Logging
- Validation
- Performance Monitoring
- Authorization
- Auditing
- Caching

Example:

```csharp
public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("Before");

        var response = await next();

        Console.WriteLine("After");

        return response;
    }
}
```

Register it.

```csharp
builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingBehavior<,>));
```

Multiple behaviors are supported and execute in the order they are registered.

---

## Architecture

```
Application
      │
      ▼
IMediator
      │
      ▼
Pipeline Behaviors
      │
      ▼
Request Handler
      │
      ▼
Business Logic
      │
      ▼
Response
```

---

## Why use Brainy.Mediator?

Using a mediator helps separate business logic from controllers, services, and UI layers. Instead of objects calling each other directly, requests are sent through a single mediator, making the application easier to maintain, test, and extend.

Brainy.Mediator focuses on the features most applications need while keeping the API simple and familiar.

---

## Roadmap

Future releases may include:

- Request Pre/Post Processors
- Streaming Requests
- Request Timeouts
- Built-in Validation Pipeline
- Performance Diagnostics
- Source Generator Support

---

## Contributing

Contributions, ideas, feature requests, and bug reports are always welcome.

If you'd like to improve Brainy.Mediator, feel free to open an issue or submit a pull request.

---

# Author

Brainy.Mapper is created and maintained by **Shakeel Iqbal**, a Senior .NET Architect and C# Developer with extensive experience building enterprise applications and software solutions using the Microsoft technology stack.

- LinkedIn: [Shakeel Iqbal](https://www.linkedin.com/in/shakeel-iqbal1/)
- Company: [Brainy Solutions](https://www.brainy-solutions.com/)

---

## License

This project is licensed under the MIT License.

---

## About Brainy Solutions

Brainy.Mediator is maintained by **Brainy Solutions**.

We build modern software solutions using .NET, cloud technologies, AI, and enterprise architecture. As part of our commitment to the developer community, we actively open-source tools and libraries that help simplify software development.

If you find Brainy.Mediator useful, feel free to ⭐ star the repository, report issues, suggest improvements, or contribute through a pull request.