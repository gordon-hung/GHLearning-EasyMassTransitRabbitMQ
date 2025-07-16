using GHLearning.EasyMassTransitRabbitMQ.FaultMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyMassTransitRabbitMQ.FaultClient.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FaultController(
	IBus bus,
	TimeProvider timeProvider) : ControllerBase
{
	[HttpPost()]
	public Task PublishAsync()
		=> bus.Publish(
			message: new OrderMessage
			{
				OrderId = Guid.CreateVersion7(timeProvider.GetUtcNow())
			},
			cancellationToken: HttpContext.RequestAborted);
}
