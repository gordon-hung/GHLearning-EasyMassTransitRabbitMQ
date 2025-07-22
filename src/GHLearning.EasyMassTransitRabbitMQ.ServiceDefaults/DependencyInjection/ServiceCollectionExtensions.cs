using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyMassTransitRabbitMQ.ServiceDefaults.DependencyInjection;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServiceDefaults(this IServiceCollection services)
		=> services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
}
