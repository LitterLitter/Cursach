using Shared.Serialization;
using static Server.Logic.DatabaseInfo;

namespace Server.Logic
{
    public class AuthenticationHandler : IHandle<Authentication>
    {
        public Response Handle(Authentication request)
        {
            if (FindUser(request.Login)!=-1)
            {
                return new AuthResponse()
                {
                    Type = ExchangeType.Authentication,
                    Code = ResponseCode.Success
                };
            }
            return new AuthResponse()
            {
                Type = ExchangeType.Authentication,
                Code = ResponseCode.Fail
            };
        }
    }
}
