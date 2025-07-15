using GHLearning.EasyMassTransitRabbitMQ.FanoutMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.FanoutServer.ConsumerHeaders;

public class LoginRecordConsumerHeader(
	ILogger<LoginRecordConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<LoginMessage>
{
	public Task Consume(ConsumeContext<LoginMessage> context)
	{
		logger.LogInformation("""
			LogAt:{logAt}
			Id:{id}
			User:{user}
			""",
			timeProvider.GetUtcNow(),
			context.Message.Id,
			context.Message.User);

		return Task.CompletedTask;
	}
}
