using Brainy.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainy.MediatorTest.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> HandleAsync(TRequest request,Func<Task<TResponse>> next,CancellationToken cancellationToken)
        {
            Console.WriteLine("Behavior Before");

            var response = await next();

            Console.WriteLine("Behavior After");

            return response;
        }
    }
}
