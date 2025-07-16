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
	[HttpPost("Deposit/Form")]
	public Task DepositFormAsync()
		=> bus.Publish(
			message: new OrderDepositFormMessage
			{
				OrderId = Guid.CreateVersion7(timeProvider.GetUtcNow())
			},
			cancellationToken: HttpContext.RequestAborted);

}
