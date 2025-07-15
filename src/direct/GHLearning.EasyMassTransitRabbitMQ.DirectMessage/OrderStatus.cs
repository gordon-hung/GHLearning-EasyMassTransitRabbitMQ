namespace GHLearning.EasyMassTransitRabbitMQ.DirectMessage;

public enum OrderStatus
{
	/// <summary>
	/// Pending (待處理)：訂單已建立，尚未開始處理。
	/// </summary>
	Pending = 1,

	/// <summary>
	/// Processing (處理中)：訂單正在被處理中。
	/// </summary>
	Processing = 2,

	/// <summary>
	/// Shipped (已發貨)：訂單已經發送，等待送達。
	/// </summary>
	Shipped = 3,

	/// <summary>
	/// Delivered (已送達)：訂單已成功送達給客戶。
	/// </summary>
	Delivered = 4,

	/// <summary>
	/// Cancelled (已取消)：訂單被取消，無法完成。
	/// </summary>
	Cancelled = 5,

	/// <summary>
	/// Refunded (已退款)：訂單已退款。
	/// </summary>
	Refunded = 6,

	/// <summary>
	/// Completed (已完成)：訂單處理完成，包含交付及支付等程序。
	/// </summary>
	Completed = 7
}
