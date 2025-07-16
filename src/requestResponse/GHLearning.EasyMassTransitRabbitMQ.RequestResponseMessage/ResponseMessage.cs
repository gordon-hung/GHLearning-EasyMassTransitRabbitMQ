namespace GHLearning.EasyMassTransitRabbitMQ.RequestResponseMessage;

public record ResponseMessage
{
	public Guid ResponseId { get; set; }
	public Guid RequestId { get; set; }
	public string Message { get; set; } = default!;
}
