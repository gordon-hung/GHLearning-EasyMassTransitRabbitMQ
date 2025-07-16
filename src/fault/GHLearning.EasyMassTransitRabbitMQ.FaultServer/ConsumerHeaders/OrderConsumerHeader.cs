using GHLearning.EasyMassTransitRabbitMQ.FaultMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.FaultServer.ConsumerHeaders;

public class OrderConsumerHeader(
	ILogger<OrderConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<OrderMessage>
{
	public Task Consume(ConsumeContext<OrderMessage> context)
	{
		logger.LogInformation("""
			LogAt:{logAt}
			OrderId:{orderId}
			""",
			timeProvider.GetUtcNow(),
			context.Message.OrderId);
		return Task.FromException(new NotImplementedException());
	}
}
