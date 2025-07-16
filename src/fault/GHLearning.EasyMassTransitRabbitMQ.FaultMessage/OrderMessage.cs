namespace GHLearning.EasyMassTransitRabbitMQ.FaultMessage;

public record OrderMessage
{
	public Guid OrderId { get; set; }
}
