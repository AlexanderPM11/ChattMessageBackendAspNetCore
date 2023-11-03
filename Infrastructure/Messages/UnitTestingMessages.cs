using crudSignalR.Core.Application.Features.Message.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crudSignal.Infrastructure.UnitTesting.Messages
{
    public class UnitTestingMessages
    {
        private readonly IMediator _mediator;

        public UnitTestingMessages(IMediator mediator) { _mediator = mediator; }
        [Fact]
        public async Task GetMessage()
        {
            //Arrage
            string UserTo = "";
            string UserFrom = "";

            //Act
            var toReturn = await _mediator.Send(new GetAllMessagesByUser { UserTo = UserTo, UserFrom = UserFrom });

            //Assert
            Assert.True(toReturn.payload != null);
        }
    }
}
