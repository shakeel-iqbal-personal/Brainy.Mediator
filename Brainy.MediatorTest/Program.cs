using Brainy.Mediator;
using Brainy.MediatorTest.Application.Behaviors;
using Brainy.MediatorTest.Application.Commands;
using Brainy.MediatorTest.Application.Handlers.Students.Commands.CreateStudent;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<IMediator, BrainyMediator>();

services.AddScoped<IRequestHandler<CreateStudentCommand, long>,CreateStudentCommandHandler>();

Console.WriteLine("--- Test Three: For pipeline behavior applying to all requests and responses ---");

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

Console.WriteLine("--- Test Four: For testing the of the pipline behaviors .Reverse() ---");

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));


var provider = services.BuildServiceProvider();

var mediator = provider.GetRequiredService<IMediator>();

Console.WriteLine("---Test One Checking SendAsync()...");

var command = new CreateStudentCommand(
    "Ali",
    "Islamabad");

var result = await mediator.SendAsync(command);

Console.WriteLine($"Returned Id : {result}");

Console.WriteLine();
Console.WriteLine("--- Test Two Checking Missing Handler ---");

try
{
    var deleteCommand = new DeleteStudentCommand
    {
        Id = 1
    };

    await mediator.SendAsync(deleteCommand);
}catch(Exception ex)
{
    Console.WriteLine($"Exception : {ex.Message}");
}   