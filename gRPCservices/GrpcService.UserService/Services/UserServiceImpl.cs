using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.UserService.Services
{
    public class UserServiceImpl : GrpcService.Protos.UserService.UserServiceBase
    {
        public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var resp = new GetUserResponse
            {
                Id = request.Id,
                Name = $"User number {request.Id}",
                Email = "examle@gmail.com"
            };

            return Task.FromResult(resp);
        }
    }
}
