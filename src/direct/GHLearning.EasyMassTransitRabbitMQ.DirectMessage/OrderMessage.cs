namespace GHLearning.EasyMassTransitRabbitMQ.DirectMessage;

public record OrderMessage
{
	public OrderStatus Status { get; set; }
	public Guid Id { get; set; }
}
