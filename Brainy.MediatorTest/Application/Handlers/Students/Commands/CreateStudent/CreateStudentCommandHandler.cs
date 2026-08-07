using Brainy.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainy.MediatorTest.Application.Handlers.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler: IRequestHandler<CreateStudentCommand, long>
    {
        public async Task<long> Handle(CreateStudentCommand request,CancellationToken cancellationToken)
        {
            Console.WriteLine("Handler Started");

            Console.WriteLine($"Name : {request.Name}");
            Console.WriteLine($"Address : {request.Address}");

            Console.WriteLine("Saving Student...");

            return await Task.FromResult(1L);
        }
    }
}
