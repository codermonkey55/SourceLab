using MediatR;
namespace MediatRSample.Objects
{
    public class PingAsync : IAsyncRequest<Pong>
    {
        public string Message { get; set; }
    }
}