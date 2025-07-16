using GHLearning.EasyMassTransitRabbitMQ.FaultMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.FaultServer.ConsumerHeaders;

public class OrderDepositFormConsumerHeader(
	ILogger<OrderDepositFormConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<OrderDepositFormMessage>
{
	public Task Consume(ConsumeContext<OrderDepositFormMessage> context)
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
