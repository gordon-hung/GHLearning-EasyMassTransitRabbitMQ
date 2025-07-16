using GHLearning.EasyMassTransitRabbitMQ.FaultMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.FaultServer.ConsumerHeaders;

public class FaultOrderConsumerHeader(
	ILogger<FaultOrderConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<Fault<OrderMessage>>
{
	public Task Consume(ConsumeContext<Fault<OrderMessage>> context)
	{
		logger.LogError("""
			LogAt:{logAt}
			OrderId:{orderId}
			""",
			timeProvider.GetUtcNow(),
			context.Message.Message.OrderId);

		return Task.CompletedTask;
	}
}
