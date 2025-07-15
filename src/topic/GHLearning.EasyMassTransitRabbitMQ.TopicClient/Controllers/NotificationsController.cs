using GHLearning.EasyMassTransitRabbitMQ.TopicClient.ViewModels;
using GHLearning.EasyMassTransitRabbitMQ.TopicMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyMassTransitRabbitMQ.TopicClient.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationsController(
	IBus bus,
	TimeProvider timeProvider) : ControllerBase
{
	[HttpPost("Group")]
	public Task NotificationGroupAsync(
		[FromBody] NotificationGroupViewModel source)
		=> bus.Publish(
			message: new NotificationMessage
			{
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
				Sender = source.Sender,
				Region = source.Region,
				Group = source.Group,
				User = string.Empty,
				Type = NotificationType.Group,
				Medium = source.Medium,
				Message = source.Message,
			});

	[HttpPost("Personal")]
	public Task NotificationPersonalAsync(
		[FromBody] NotificationPersonalViewModel source)
		=> bus.Publish(
			message: new NotificationMessage
			{
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
				Sender = source.Sender,
				Region = source.Region,
				Group = source.Group,
				User = source.User,
				Type = NotificationType.Personal,
				Medium = source.Medium,
				Message = source.Message,
			});

	[HttpPost("Public")]
	public Task NotificationPublicAsync(
		[FromBody] NotificationPublicViewModel source)
		=> bus.Publish(
			message: new NotificationMessage
			{
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
				Sender = source.Sender,
				Region = source.Region,
				Group = string.Empty,
				User = string.Empty,
				Type = NotificationType.Public,
				Medium = source.Medium,
				Message = source.Message,
			});
}
