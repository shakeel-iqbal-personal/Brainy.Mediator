using Brainy.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainy.MediatorTest.Application.Commands
{
    public class DeleteStudentCommand : IRequest<int>
    {
        public long Id { get; set; }
    }
}
