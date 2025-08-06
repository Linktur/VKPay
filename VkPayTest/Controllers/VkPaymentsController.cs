using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VkPayTest.Models;
using VkPayTest.Services;

namespace VkPayTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VkPaymentsController : ControllerBase
    {
        private readonly IVkPaymentService _paymentService;
        private readonly ILogger<VkPaymentsController> _logger;
        private readonly VkSettings _vkSettings;

        public VkPaymentsController(
            IVkPaymentService paymentService,
            ILogger<VkPaymentsController> logger,
            IOptions<VkSettings> vkSettings)
        {
            _paymentService = paymentService;
            _logger = logger;
            _vkSettings = vkSettings.Value;
        }

        /// <summary>
        /// Main webhook endpoint for VK payment notifications
        /// </summary>
        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromForm] VkPaymentNotification notification)
        {
            _logger.LogInformation("Received VK payment notification: {NotificationType}", notification.NotificationType);

            // Verify the signature if provided
            if (!string.IsNullOrEmpty(notification.Sig))
            {
                var form = await Request.ReadFormAsync();
                var notificationData = string.Join("&", form.Keys
                    .Where(k => k != "sig")
                    .OrderBy(k => k)
                    .Select(k => $"{k}={form[k]}"));

                if (!_paymentService.VerifySignature(notification.Sig, notificationData))
                {
                    _logger.LogWarning("Invalid signature for payment notification");
                    return BadRequest("Invalid signature");
                }
            }

            // Process notification and get response
            var response = await _paymentService.ProcessPaymentNotificationAsync(notification);
            
            // Return different responses based on notification type
            switch (notification.NotificationType)
            {
                case "get_item":
                    var itemResponse = await _paymentService.HandleGetItemAsync(notification);
                    return Ok(new { response = itemResponse });
                case "get_subscription":
                    return Ok("ok"); // пока просто ок
                case "order_status_change":
                    var statusResponse = await _paymentService.HandleOrderStatusChangeAsync(notification);
                    return Ok(statusResponse);
                default:
                    return Ok("ok");
            }
        }

        /// <summary>
        /// Test endpoint for VK payment notifications with minimal validation
        /// </summary>
        [HttpPost("test-callback")]
        public async Task<IActionResult> TestCallback([FromBody] TestNotificationDto notification)
        {
            _logger.LogInformation("Received test VK payment notification: {NotificationType}", notification.NotificationType);

            // Create a full notification from minimal data
            var fullNotification = new VkPaymentNotification
            {
                Type = notification.Type ?? "order",
                NotificationType = notification.NotificationType ?? "get_item",
                AppId = notification.AppId ?? _vkSettings.AppId,
                UserId = notification.UserId ?? "test_user",
                ReceiverId = "test_receiver",
                OrderId = "test_order_" + DateTime.Now.Ticks,
                Item = notification.Item ?? "test_item",
                ItemTitle = "Test Item",
                ItemPhotoUrl = "https://example.com/test.jpg",
                Price = notification.Price ?? 100,
                Currency = "RUB",
                Status = "chargeable",
                SubscriptionId = "test_sub_" + DateTime.Now.Ticks,
                PaymentStatus = "pending",
                Sig = "test_signature",
                RawRequest = System.Text.Json.JsonSerializer.Serialize(notification),
                CreatedAt = DateTime.UtcNow
            };

            // Process notification and get response
            var response = await _paymentService.ProcessPaymentNotificationAsync(fullNotification);
            
            // Return different responses based on notification type
            switch (fullNotification.NotificationType)
            {
                case "get_item":
                    var itemResponse = await _paymentService.HandleGetItemAsync(fullNotification);
                    return Ok(new { response = itemResponse });
                case "get_subscription":
                    return Ok("ok"); // пока просто ок
                case "order_status_change":
                    var statusResponse = await _paymentService.HandleOrderStatusChangeAsync(fullNotification);
                    return Ok(statusResponse);
                default:
                    return Ok("ok");
            }
        }

        /// <summary>
        /// Endpoint for VKWebAppShowOrderBox
        /// </summary>
        [HttpGet("order-box")]
        public async Task<IActionResult> GetOrderBox([FromQuery] string userId, [FromQuery] string itemId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(itemId))
            {
                return BadRequest("userId and itemId are required");
            }

            var result = await _paymentService.HandleOrderBoxAsync(userId, itemId);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint for VKWebAppShowSubscriptionBox
        /// </summary>
        /// <summary>
        /// Endpoint for VKWebAppShowSubscriptionBox
        /// </summary>
        /// <param name="userId">VK user ID</param>
        /// <param name="action">Action to perform (create, cancel, resume)</param>
        /// <param name="subscriptionId">Subscription ID (required for cancel/resume)</param>
        /// <returns>Subscription operation result</returns>
        [HttpPost("subscription-box")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HandleSubscriptionBox(
            [FromQuery, Required] string userId,
            [FromQuery, Required] string action,
            [FromQuery] string subscriptionId = null)
        {
            try
            {
                _logger.LogInformation("Processing subscription box request: {Action} for user {UserId}", action, userId);
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(new 
                    { 
                        error = "invalid_parameters",
                        error_description = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Validate action parameter
                var validActions = new[] { "create", "cancel", "resume" };
                if (!validActions.Contains(action.ToLower()))
                {
                    return BadRequest(new 
                    { 
                        error = "invalid_action",
                        error_description = "Invalid action. Must be one of: create, cancel, resume"
                    });
                }

                // For cancel and resume actions, subscriptionId is required
                if ((action == "cancel" || action == "resume") && string.IsNullOrEmpty(subscriptionId))
                {
                    return BadRequest(new 
                    { 
                        error = "subscription_id_required",
                        error_description = "Subscription ID is required for cancel/resume actions"
                    });
                }

                var result = await _paymentService.HandleSubscriptionBoxAsync(action, userId, subscriptionId);
                return Ok(new { response = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing subscription box request");
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    error = "internal_server_error",
                    error_description = "An error occurred while processing the request"
                });
            }
        }
    }

    /// <summary>
    /// Simplified DTO for testing VK payment notifications
    /// </summary>
    public class TestNotificationDto
    {
        public string Type { get; set; }
        public string NotificationType { get; set; }
        public string AppId { get; set; }
        public string UserId { get; set; }
        public string Item { get; set; }
        public decimal? Price { get; set; }
    }
}
