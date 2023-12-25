namespace Basket.Api.Entities;

public record ShoppingCartItem(int Quantity, decimal Price, Guid ProducId, string ProductName);