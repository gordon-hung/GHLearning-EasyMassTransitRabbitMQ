using RabbitMQ.Client;

namespace GHLearning.EasyMassTransitRabbitMQ.ServiceDefaults;

internal sealed class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
	private readonly Dictionary<string, IConnection> _connections = [];
	public bool Add(string name, IConnection connection) => _connections.TryAdd(name, connection);
	public IConnection? Get(string name) => _connections.TryGetValue(name, out var connection) ? connection : null;
}
