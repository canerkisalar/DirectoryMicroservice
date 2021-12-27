using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Moq;

namespace Phonebook.Services.Report.Tests.Mock
{
    public static class SendEndpointFactory
    {
        public static Mock<ISendEndpointProvider> GetSendEndpoint()
        {
           var _mockSendEndpoint = new Mock<ISendEndpoint>();

            var mockSendEndpointProvider = new Mock<ISendEndpointProvider>();
            mockSendEndpointProvider.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>())).Returns(Task.FromResult(_mockSendEndpoint.Object));
            return mockSendEndpointProvider;
        }
    }
}
