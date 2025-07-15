using GHLearning.EasyMassTransitRabbitMQ.FanoutClient.ViewModels;
using GHLearning.EasyMassTransitRabbitMQ.FanoutMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyMassTransitRabbitMQ.FanoutClient.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LoginController(
	IBus bus,
	TimeProvider timeProvider) : ControllerBase
{
	[HttpPost()]
	public Task NotificationGroupAsync(
		[FromBody] LoginViewModel source)
		=> bus.Publish(
			message: new LoginMessage
			{
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
				User = source.User
			});
}
