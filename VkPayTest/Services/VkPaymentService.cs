using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VkPayTest.Data;
using VkPayTest.Models;

namespace VkPayTest.Services
{
    public class VkPaymentService : IVkPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VkPaymentService> _logger;
        private readonly VkSettings _vkSettings;

        public VkPaymentService(
            ApplicationDbContext context,
            ILogger<VkPaymentService> logger,
            IOptions<VkSettings> vkSettings)
        {
            _context = context;
            _logger = logger;
            _vkSettings = vkSettings.Value;
        }

        public async Task<bool> ValidateNotificationAsync(VkPaymentNotification notification)
        {
            if (notification == null)
                return false;

            // Basic validation
            if (string.IsNullOrEmpty(notification.Type) || 
                string.IsNullOrEmpty(notification.NotificationType) ||
                string.IsNullOrEmpty(notification.AppId) || 
                string.IsNullOrEmpty(notification.UserId))
            {
                _logger.LogWarning("Invalid notification: Missing required fields");
                return false;
            }

            // Verify app ID matches
            if (notification.AppId != _vkSettings.AppId)
            {
                _logger.LogWarning($"AppId mismatch. Expected: {_vkSettings.AppId}, Got: {notification.AppId}");
                return false;
            }

            return true;
        }

        public async Task<object> ProcessPaymentNotificationAsync(VkPaymentNotification notification)
        {
            if (!await ValidateNotificationAsync(notification))
            {
                _logger.LogWarning("Invalid payment notification received");
                return new { error = "invalid_notification" };
            }

            // Log the notification
            _context.PaymentNotifications.Add(notification);
            await _context.SaveChangesAsync();

            try
            {
                switch (notification.NotificationType.ToLower())
                {
                    case "get_item":
                        return await HandleGetItemAsync(notification);
                    case "get_subscription":
                        await HandleGetSubscription(notification);
                        return "ok";
                    case "order_status_change":
                        return await HandleOrderStatusChangeAsync(notification);
                    default:
                        _logger.LogWarning($"Unhandled notification type: {notification.NotificationType}");
                        return "ok";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment notification");
                throw;
            }
        }

        public async Task<object> HandleOrderBoxAsync(string userId, string itemId)
        {
            // In a real application, you would validate the item exists and get its details
            // For now, we'll return a mock response
            return new
            {
                type = "item",
                title = "Test Item",
                photo_url = "https://example.com/item.jpg",
                price = 10,
                item_id = itemId,
                description = "A test item for VK Pay integration"
            };
        }

        public async Task<object> HandleSubscriptionBoxAsync(string action, string userId, string subscriptionId = null)
        {
            // In a real application, you would handle subscription creation, cancellation, etc.
            // For now, we'll return a mock response
            return new
            {
                success = true,
                subscription_id = subscriptionId ?? Guid.NewGuid().ToString(),
                status = action == "create" ? "active" : "canceled"
            };
        }

        public bool VerifySignature(string signature, string notificationData)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(notificationData))
                return false;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_vkSettings.ValidationKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(notificationData));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            
            return string.Equals(hashString, signature, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<object> HandleGetItemAsync(VkPaymentNotification notification)
        {
            _logger.LogInformation($"Processing get_item for item: {notification.Item}");
            
            try
            {
                // Получаем товар из базы данных
                var item = await _context.VkItems
                    .FirstOrDefaultAsync(x => x.Id == notification.Item && x.IsActive);
                
                if (item == null)
                {
                    _logger.LogWarning($"Item not found or inactive: {notification.Item}");
                    return new { error = "item_not_found" };
                }
                
                // Возвращаем информацию о товаре для VK
                return new
                {
                    title = item.TitleRu,
                    photo_url = item.PhotoUrl ?? "https://lyboe.ru/images/default-item.jpg",
                    price = (int)item.Price, // VK ожидает цену в копейках, но мы храним в рублях
                    item_id = item.Id,
                    description = item.DescriptionRu ?? item.TitleRu
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting item {notification.Item}");
                return new { error = "internal_error" };
            }
        }

        public async Task<object> HandleOrderStatusChangeAsync(VkPaymentNotification notification)
        {
            _logger.LogInformation($"Processing order_status_change for order: {notification.OrderId}, status: {notification.Status}");
            
            try
            {
                // В реальном приложении здесь бы было обновление статуса заказа в базе данных
                // Пример:
                // var order = await _context.Orders.FindAsync(notification.OrderId);
                // if (order == null)
                // {
                //     _logger.LogWarning($"Order not found: {notification.OrderId}");
                //     return new { error = "order_not_found" };
                // }
                // 
                // order.Status = notification.Status;
                // order.UpdatedAt = DateTime.UtcNow;
                // await _context.SaveChangesAsync();
                
                // Логика обработки успешной покупки
                if (notification.Status?.ToLower() == "purchased")
                {
                    _logger.LogInformation($"Order {notification.OrderId} successfully purchased by user {notification.UserId}");
                    
                    // Здесь можно добавить логику выдачи товара игроку
                    // Например: await _gameService.GiveItemToPlayerAsync(notification.UserId, notification.Item);
                    
                    // Возвращаем положительный ответ VK с статусом chargeable
                    return new { status = "chargeable" };
                }
                else if (notification.Status?.ToLower() == "canceled")
                {
                    _logger.LogInformation($"Order {notification.OrderId} was canceled");
                    return new { status = "canceled" };
                }
                
                // Для других статусов возвращаем общий ответ
                return new { status = notification.Status ?? "unknown" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order status change for order {notification.OrderId}");
                return new { error = "processing_error" };
            }
        }

        #region Private Methods

        private async Task HandleGetSubscription(VkPaymentNotification notification)
        {
            // Implement logic to get subscription details
            _logger.LogInformation($"Processing get_subscription for user: {notification.UserId}");
        }

        #endregion
    }
}
