using Shared.Serialization;
using static Server.Logic.DatabaseInfo;

namespace Server.Logic
{
    public class RegistrationHandler : IHandle<Registration>
    {
        public Response Handle(Registration request)
        {
            if (FindUser(request.Login) == -1)
            {
                AddUser(request.Login, request.Password);
                return new RegResponse()
                {
                    Type = ExchangeType.Registration,
                    Code = ResponseCode.Success
                };
            }
            return new RegResponse()
            {
                Type = ExchangeType.Registration,
                Code = ResponseCode.Fail
            };
        }

    }
}

