namespace GHLearning.EasyMassTransitRabbitMQ.RequestResponseMessage;
public record RequestMessage
{
	public Guid Id { get; set; }
	public string Message { get; set; } = default!;
}
