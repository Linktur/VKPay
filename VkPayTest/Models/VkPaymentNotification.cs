using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VkPayTest.Models
{
    public class VkPaymentNotification : BaseEntity
    {
        [Required]
        public string Type { get; set; } // order, subscription, etc.
        
        [Required]
        public string NotificationType { get; set; } // get_item, get_subscription, order_status_change, etc.
        
        [Required]
        public string AppId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public string ReceiverId { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string Item { get; set; } = ""; // Item ID or subscription ID
        public string ItemTitle { get; set; } = "";
        public string ItemPhotoUrl { get; set; } = "";
        public decimal? Price { get; set; }
        public string Currency { get; set; } = "RUB";
        public string Status { get; set; } = ""; // chargeable, purchased, canceled, etc.
        public string SubscriptionId { get; set; } = ""; // For subscriptions
        public DateTime? ExpiresAt { get; set; } // For subscriptions
        public DateTime? CanceledAt { get; set; } // For subscriptions
        public string PaymentStatus { get; set; } = "";
        public string Sig { get; set; } = ""; // VK signature for verification
        
        // Raw request data for debugging
        public string RawRequest { get; set; } = "{}";
        
        // Additional data
        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}
