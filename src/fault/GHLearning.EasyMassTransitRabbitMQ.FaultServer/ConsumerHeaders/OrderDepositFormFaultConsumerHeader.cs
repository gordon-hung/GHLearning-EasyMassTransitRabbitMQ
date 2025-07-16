using GHLearning.EasyMassTransitRabbitMQ.FaultMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.FaultServer.ConsumerHeaders;

public class OrderDepositFormFaultConsumerHeader(
	ILogger<OrderDepositFormFaultConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<Fault<OrderDepositFormMessage>>
{
	public Task Consume(ConsumeContext<Fault<OrderDepositFormMessage>> context)
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
