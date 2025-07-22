using RabbitMQ.Client;

namespace GHLearning.EasyMassTransitRabbitMQ.ServiceDefaults;
public interface IRabbitMQConnectionFactory
{
	bool Add(string name, IConnection connection);
	IConnection? Get(string name);
}
