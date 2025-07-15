namespace GHLearning.EasyMassTransitRabbitMQ.TopicMessage;

public record NotificationMessage
{
	// 訊息的唯一識別碼
	public Guid Id { get; set; }

	// 發送者名稱或發送者 ID，代表發送通知的人或系統
	public string Sender { get; set; } = default!;

	// 訊息所屬的地區，可能用來標識通知的範圍或地理區域（例如，台灣區）
	public string Region { get; set; } = default!;

	// 目標群組名稱，指的是接收通知的群體（例如，市場部、VIP 客戶）
	public string Group { get; set; } = default!;

	// 用戶名稱，表示收到通知的特定用戶（例如，某個員工或顧客）
	public string User { get; set; } = default!;

	// 訊息的類型，指示這條通知的性質（例如，個人通知、群組通知、公開通知等）
	public NotificationType Type { get; set; }

	// 訊息發送的通知管道，指的是訊息發送的具體方式（例如，Email、SMS、Line）
	public NotificationMedium Medium { get; set; }

	// 訊息的具體內容，發送的通知正文
	public string Message { get; set; } = default!;
}
