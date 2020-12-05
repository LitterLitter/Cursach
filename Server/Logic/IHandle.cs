using Shared.Serialization;

namespace Server.Logic
{
    public interface IHandle<TRequest>
        where TRequest : Request
    {
        Response Handle(TRequest request);
    }
}

