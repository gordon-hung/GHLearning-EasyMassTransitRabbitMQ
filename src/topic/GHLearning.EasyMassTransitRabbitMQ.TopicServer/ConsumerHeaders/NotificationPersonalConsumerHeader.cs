using GHLearning.EasyMassTransitRabbitMQ.TopicMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.TopicServer.ConsumerHeaders;

public class NotificationPersonalConsumerHeader(
	ILogger<NotificationPersonalConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<NotificationMessage>
{
	public async Task Consume(ConsumeContext<NotificationMessage> context)
	{
		logger.LogInformation("""
			LogAt:{logAt}
			Id:{id}
			Sender:{sender}
			Region:{region}
			Group:{group}
			User:{user}
			Type:{type}
			Medium:{medium}
			Message:{message}
			""",
			timeProvider.GetUtcNow(),
			context.Message.Id,
			context.Message.Sender,
			context.Message.Region,
			context.Message.Group,
			context.Message.User,
			context.Message.Type,
			context.Message.Medium,
			context.Message.Message);

		await Task.Delay(
			delay: TimeSpan.FromSeconds(1),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);
	}
}
