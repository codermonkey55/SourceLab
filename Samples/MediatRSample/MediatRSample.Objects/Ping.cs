using MediatR;
namespace MediatRSample.Objects
{
    public class Ping : IRequest<Pong>
    {
        public string Message { get; set; }
    }
}