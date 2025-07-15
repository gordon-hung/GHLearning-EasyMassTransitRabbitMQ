using GHLearning.EasyMassTransitRabbitMQ.DirectMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.DirectServer.ConsumerHeaders;

public class OrderCompletedConsumerHeader(
	ILogger<OrderCompletedConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<OrderMessage>
{
	public async Task Consume(ConsumeContext<OrderMessage> context)
	{
		logger.LogInformation("""
			LogAt:{logAt}
			Id:{id}
			Status:{status}
			""",
			timeProvider.GetUtcNow(),
			context.Message.Id,
			context.Message.Status);

		await Task.Delay(
			delay: TimeSpan.FromSeconds(1),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);
	}
}
