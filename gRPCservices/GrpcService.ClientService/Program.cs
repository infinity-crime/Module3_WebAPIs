using Grpc.Net.Client;
using GrpcService.Protos;

var channel = GrpcChannel.ForAddress("https://localhost:7062");
var client = new UserService.UserServiceClient(channel);

var response = client.GetUser(new GetUserRequest { Id = 100 });

Console.WriteLine($"Id = {response.Id}, Name = {response.Name}, Email = {response.Email}");
