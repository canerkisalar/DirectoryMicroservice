using System;
using System.Threading.Tasks;
using MassTransit;
using Moq;

namespace Phonebook.Services.Phonebook.Test.Mock
{
    public static class SendEndpointFactory
    {
        public static Mock<ISendEndpointProvider> GetSendEndpoint()
        {
           var mockSendEndpoint = new Mock<ISendEndpoint>();

            var mockSendEndpointProvider = new Mock<ISendEndpointProvider>();
            mockSendEndpointProvider.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>())).Returns(Task.FromResult(mockSendEndpoint.Object));
            return mockSendEndpointProvider;
        }
    }
}
