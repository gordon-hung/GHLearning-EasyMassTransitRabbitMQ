using GHLearning.EasyMassTransitRabbitMQ.DirectMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyMassTransitRabbitMQ.DirectClient.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(
	TimeProvider timeProvider) : ControllerBase
{
	[HttpPost]
	public Task CreatedAsync(
		[FromServices] IBus bus)
		=> bus.Publish(
			message: new OrderMessage
			{
				Status = OrderStatus.Pending,
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow())
			},
			cancellationToken: HttpContext.RequestAborted);
}
