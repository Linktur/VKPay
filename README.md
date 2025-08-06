# VK Pay Integration for .NET

This project provides a .NET implementation for handling VK Pay integration, including payment notifications, order boxes, and subscription management.

## Features

- ✅ VK Pay notification webhook handler
- ✅ Order box implementation (VKWebAppShowOrderBox)
- ✅ Subscription box implementation (VKWebAppShowSubscriptionBox)
- ✅ Secure request validation with signature verification
- ✅ Comprehensive error handling and logging
- ✅ Swagger API documentation
- ✅ Entity Framework Core integration

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB is used by default)
- VK App ID and Secret Key

## Configuration

1. Update `appsettings.json` with your VK application credentials:

```json
{
  "VkSettings": {
    "AppId": "YOUR_APP_ID",
    "AppSecret": "YOUR_APP_SECRET",
    "ValidationKey": "YOUR_VALIDATION_KEY",
    "IsSandbox": true
  }
}
```

2. Configure your database connection string in `appsettings.json` if needed.

## Running the Application

1. Restore NuGet packages:
   ```
   dotnet restore
   ```

2. Apply database migrations:
   ```
   dotnet ef database update
   ```

3. Run the application:
   ```
   dotnet run
   ```

4. Access the Swagger UI at `https://localhost:5001` (or your configured URL)

## API Endpoints

### Payment Notification Webhook

- **URL**: `POST /api/vkpayments/callback`
- **Content-Type**: `application/x-www-form-urlencoded`
- **Description**: Handles all VK payment notifications

### Order Box

- **URL**: `GET /api/vkpayments/order-box?userId={userId}&itemId={itemId}`
- **Description**: Returns item details for the order box

### Subscription Box

- **URL**: `POST /api/vkpayments/subscription-box?userId={userId}&action={action}&subscriptionId={subscriptionId}`
- **Description**: Handles subscription operations (create, cancel, resume)

## Testing

1. Set `IsSandbox` to `true` in `appsettings.json`
2. Add test users in your VK application settings
3. Use the VK Bridge methods in your frontend:

```javascript
// Example: Show order box
bridge.send('VKWebAppShowOrderBox', { 
  type: 'item',
  item: 'item_id_123456'
}).then(data => {
  if (data.success) {
    console.log('Order successful');
  }
});
```

## Security Considerations

- Always verify request signatures in production
- Keep your VK App Secret and Validation Key secure
- Use HTTPS in production
- Implement rate limiting
- Monitor logs for suspicious activities

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
