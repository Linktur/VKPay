using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VkPayTest.Models
{
    public class VkPaymentNotification
    {
        public string? Type { get; set; } // order, subscription, etc.
        
        [Required]
        public string? NotificationType { get; set; } // get_item, get_subscription, order_status_change, etc.
        
        public string? AppId { get; set; }
        
        public string? UserId { get; set; }
        
        public string? ReceiverId { get; set; }
        
        public string? OrderId { get; set; }
        
        [Required]
        public string? Item { get; set; } // Item ID or subscription ID
        
        public string? ItemTitle { get; set; }
        
        public string? ItemPhotoUrl { get; set; }
        
        public int? Price { get; set; }
        
        public string? Currency { get; set; }
        
        public string? Status { get; set; } // chargeable, purchased, canceled, etc.
        
        public string? SubscriptionId { get; set; } // For subscriptions
        
        public string? PaymentStatus { get; set; }
        
        public string? Sig { get; set; } // VK signature for verification
        
        public string? RawRequest { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Additional data
        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}
