using GHLearning.EasyMassTransitRabbitMQ.RequestResponseMessage;
using MassTransit;

namespace GHLearning.EasyMassTransitRabbitMQ.RequestResponseServer.ConsumerHeaders;

public class RequestResponseConsumerHeader(
	ILogger<RequestResponseConsumerHeader> logger,
	TimeProvider timeProvider) : IConsumer<RequestMessage>
{
	public async Task Consume(ConsumeContext<RequestMessage> context)
	{
		logger.LogInformation("""
			LogAt:{logAt}
			Id:{id}
			Message:{message}
			""",
			timeProvider.GetUtcNow(),
			context.Message.Id,
			context.Message.Message);

		var response = new ResponseMessage
		{
			RequestId = context.Message.Id,
			ResponseId = Guid.CreateVersion7(timeProvider.GetUtcNow()),
			Message = "Hi"
		};

		await context.RespondAsync(response).ConfigureAwait(false);
	}
}
