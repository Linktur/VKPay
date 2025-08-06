using System.Threading.Tasks;
using VkPayTest.Models;

namespace VkPayTest.Services
{
    public interface IVkPaymentService
    {
        /// <summary>
        /// Validates the incoming VK payment notification
        /// </summary>
        Task<bool> ValidateNotificationAsync(VkPaymentNotification notification);
        
        /// <summary>
        /// Processes a payment notification from VK
        /// </summary>
        Task<object> ProcessPaymentNotificationAsync(VkPaymentNotification notification);
        
        /// <summary>
        /// Handles get_item request from VK
        /// </summary>
        Task<object> HandleGetItemAsync(VkPaymentNotification notification);
        
        /// <summary>
        /// Handles order_status_change request from VK
        /// </summary>
        Task<object> HandleOrderStatusChangeAsync(VkPaymentNotification notification);
        
        /// <summary>
        /// Handles the order box payment flow
        /// </summary>
        Task<object> HandleOrderBoxAsync(string userId, string itemId);
        
        /// <summary>
        /// Handles subscription box operations (create, cancel, resume)
        /// </summary>
        Task<object> HandleSubscriptionBoxAsync(string action, string userId, string subscriptionId = null);
        
        /// <summary>
        /// Verifies the VK signature for secure communication
        /// </summary>
        bool VerifySignature(string signature, string notificationData);
    }
}
