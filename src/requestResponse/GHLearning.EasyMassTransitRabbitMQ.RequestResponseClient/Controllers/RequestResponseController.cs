using GHLearning.EasyMassTransitRabbitMQ.RequestResponseMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyMassTransitRabbitMQ.RequestResponseClient.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RequestResponseController(
	TimeProvider timeProvider) : ControllerBase
{
	[HttpPost]
	public async Task<ResponseMessage> CreatedAsync(
		[FromServices] IRequestClient<RequestMessage> client)
	{
		var request = client.Create(
			message: new RequestMessage
			{
				Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
				Message = "Hello, this is a request message!"
			},
			cancellationToken: HttpContext.RequestAborted);
		return (await request.GetResponse<ResponseMessage>().ConfigureAwait(false)).Message;
	}
}