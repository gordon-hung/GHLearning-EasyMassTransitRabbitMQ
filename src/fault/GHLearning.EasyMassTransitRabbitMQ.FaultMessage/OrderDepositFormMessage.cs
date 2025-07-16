namespace GHLearning.EasyMassTransitRabbitMQ.FaultMessage;

public record OrderDepositFormMessage
{
	public Guid OrderId { get; set; }
}
