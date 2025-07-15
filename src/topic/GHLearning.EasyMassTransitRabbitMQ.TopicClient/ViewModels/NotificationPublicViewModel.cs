using GHLearning.EasyMassTransitRabbitMQ.TopicMessage;

namespace GHLearning.EasyMassTransitRabbitMQ.TopicClient.ViewModels;

public record NotificationPublicViewModel
{
	// 發送者名稱或發送者 ID，代表發送通知的人或系統
	public string Sender { get; set; } = default!;

	// 訊息所屬的地區，可能用來標識通知的範圍或地理區域（例如，台灣區）
	public string Region { get; set; } = default!;

	// 訊息發送的通知管道，指的是訊息發送的具體方式（例如，Email、SMS、Line）
	public NotificationMedium Medium { get; set; }

	// 訊息的具體內容，發送的通知正文
	public string Message { get; set; } = default!;
}
