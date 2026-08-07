using Brainy.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainy.MediatorTest.Application.Handlers.Students.Commands.CreateStudent
{
    public sealed record CreateStudentCommand(string Name,string Address) : IRequest<long>;
}
