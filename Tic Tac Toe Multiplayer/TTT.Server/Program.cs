using Microsoft.Extensions.DependencyInjection;
using TTT.Server;
using TTT.Server.Infrastructure;

var _serviceProvider = Container.Configure();
var _server = _serviceProvider.GetRequiredService<NetworkServer>();
_server.Start();

while (true)
{
    _server.PollEvents();
    Thread.Sleep(15);
}