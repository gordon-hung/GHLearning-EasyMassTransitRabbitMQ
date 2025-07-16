using System.Net.Mime;
using System.Reflection;
using System.Text.Json.Serialization;
using CorrelationId;
using GHLearning.EasyMassTransitRabbitMQ.RequestResponseMessage;
using GHLearning.EasyMassTransitRabbitMQ.RequestResponseServer.ConsumerHeaders;
using MassTransit;
using MassTransit.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using RabbitMQ.Client;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers(options =>
	{
		options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
		options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
	})
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton(TimeProvider.System);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring  MassTransit.RabbitMQ at https://masstransit.io/documentation/transports/rabbitmq
builder.Services.AddMassTransit(registrationConfigurator =>
{
	registrationConfigurator.AddConsumers(Assembly.GetEntryAssembly());
	registrationConfigurator.UsingRabbitMq((context, configurator) =>
	{
		configurator.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));

		var exchangeName = $"{builder.Environment.EnvironmentName}.GHLearning.EasyMassTransitRabbitMQ.RequestResponse";

		configurator.Message<RequestMessage>(e => e.SetEntityName(exchangeName)); // name of the primary exchange
		configurator.Publish<RequestMessage>(e =>
		{
			e.Durable = true; // default: true
			e.AutoDelete = false; // default: false
			e.ExchangeType = ExchangeType.Direct;
		});
		configurator.Send<RequestMessage>(e =>
			// multiple conventions can be set, in this case also CorrelationId
			e.UseCorrelationId(context => context.Id));

		configurator.ReceiveEndpoint(
			queueName: $"{builder.Environment.EnvironmentName}.GHLearning.EasyMassTransitRabbitMQ.RequestResponse.Message",
			configureEndpoint: endpointConfigurator =>
			{
				endpointConfigurator.ConfigureConsumeTopology = false;
				endpointConfigurator.SetQuorumQueue();
				endpointConfigurator.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)));
				endpointConfigurator.Consumer<RequestResponseConsumerHeader>(context);
				endpointConfigurator.DiscardFaultedMessages();
				endpointConfigurator.Bind(
					exchangeName: exchangeName,
					callback: s => s.ExchangeType = ExchangeType.Direct);
			});
	});
});

//Learn more about configuring HttpLogging at https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-8.0
builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestHeaders.Add(CorrelationIdOptions.DefaultHeader);
	logging.ResponseHeaders.Add(CorrelationIdOptions.DefaultHeader);
	logging.RequestHeaders.Add(HeaderNames.TraceParent);
	logging.ResponseHeaders.Add(HeaderNames.TraceParent);
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
	logging.CombineLogs = true;
});

//Learn more about configuring OpenTelemetry at https://learn.microsoft.com/zh-tw/dotnet/core/diagnostics/observability-with-otel
builder.Services.AddOpenTelemetry()
	.ConfigureResource(resource => resource
	.AddService(
		serviceName: builder.Configuration["ServiceName"]!.ToLower(),
		serviceNamespace: typeof(Program).Assembly.GetName().Name,
		serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown"))
	.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(builder.Configuration["OtelExporterOtlpEndpoint"]!))
	.WithMetrics(metrics => metrics
		.AddMeter("GHLearning.")
		.AddAspNetCoreInstrumentation()
		.AddRuntimeInstrumentation()
		.AddProcessInstrumentation()
		.AddPrometheusExporter())
	.WithTracing(tracing => tracing
		.AddSource(DiagnosticHeaders.DefaultListenerName)
		.AddHttpClientInstrumentation()
		.AddAspNetCoreInstrumentation(options => options.Filter = (httpContext) =>
				!httpContext.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/live", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/healthz", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/metrics", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/favicon.ico", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.Value!.Equals("/api/events/raw", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.Value!.EndsWith(".js", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/_vs", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase) &&
				!httpContext.Request.Path.StartsWithSegments("/scalar", StringComparison.OrdinalIgnoreCase)));

//Learn more about configuring HealthChecks at https://learn.microsoft.com/zh-tw/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-9.0
builder.Services
	.AddHealthChecks()
	.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"])
	.AddRabbitMQ(sp =>
	{
		var factory = new ConnectionFactory
		{
			Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!)
		};
		return factory.CreateConnectionAsync();
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));// swagger/
	app.UseReDoc(options => options.SpecUrl("/openapi/v1.json"));//api-docs/
	app.MapScalarApiReference();//scalar/v1
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/live", new HealthCheckOptions
{
	Predicate = check => check.Tags.Contains("live"),
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
	Predicate = _ => true,
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});

// Prometheus 提供服務數據資料源
app.MapMetrics();
app.UseHttpMetrics();

app.Run();
