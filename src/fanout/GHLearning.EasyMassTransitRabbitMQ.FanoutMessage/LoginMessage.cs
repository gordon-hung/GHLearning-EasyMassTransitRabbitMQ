namespace GHLearning.EasyMassTransitRabbitMQ.FanoutMessage;

public record LoginMessage
{
	public Guid Id { get; set; }
	public string User { get; set; } = default!;
}
