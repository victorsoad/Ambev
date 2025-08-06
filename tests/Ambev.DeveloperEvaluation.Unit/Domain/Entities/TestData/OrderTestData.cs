using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Order entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class OrderTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Order entities.
    /// </summary>
    private static readonly Faker<Order> orderFaker = new Faker<Order>()
        .RuleFor(o => o.Id, f => f.Random.Guid())
        .RuleFor(o => o.UsuarioId, f => f.Random.Guid())
        .RuleFor(o => o.ValorTotal, f => f.Random.Decimal(10.00m, 1000.00m))
        .RuleFor(o => o.Status, f => f.PickRandom(OrderStatus.Criado, OrderStatus.Pago, OrderStatus.Cancelado))
        .RuleFor(o => o.DataCriacao, f => f.Date.Past())
        .RuleFor(o => o.DataAtualizacao, f => f.Date.Recent())
        .RuleFor(o => o.Itens, f => GenerateOrderItems(f.Random.Number(1, 5)));

    /// <summary>
    /// Configures the Faker to generate valid OrderProduct entities.
    /// </summary>
    private static readonly Faker<OrderProduct> orderProductFaker = new Faker<OrderProduct>()
        .RuleFor(op => op.Id, f => f.Random.Guid())
        .RuleFor(op => op.OrderId, f => f.Random.Guid())
        .RuleFor(op => op.ProductId, f => f.Random.Guid())
        .RuleFor(op => op.Quantidade, f => f.Random.Number(1, 20))
        .RuleFor(op => op.PrecoUnitario, f => f.Random.Decimal(1.00m, 100.00m))
        .RuleFor(op => op.DataCriacao, f => f.Date.Past());

    /// <summary>
    /// Generates a list of order items for testing.
    /// </summary>
    /// <param name="count">Number of items to generate.</param>
    /// <returns>A list of OrderProduct.</returns>
    private static List<OrderProduct> GenerateOrderItems(int count)
    {
        var items = new List<OrderProduct>();
        for (int i = 0; i < count; i++)
        {
            items.Add(new OrderProduct
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantidade = new Faker().Random.Number(1, 10),
                PrecoUnitario = new Faker().Random.Decimal(1.00m, 50.00m),
                DataCriacao = DateTime.UtcNow
            });
        }
        return items;
    }

    /// <summary>
    /// Generates a valid Order entity with randomized data.
    /// </summary>
    /// <returns>A valid Order entity with randomly generated data.</returns>
    public static Order GenerateValidOrder()
    {
        return orderFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid Order entity (empty or null values).
    /// </summary>
    /// <returns>An invalid Order entity.</returns>
    public static Order GenerateInvalidOrder()
    {
        return new Order
        {
            Id = Guid.Empty,
            UsuarioId = Guid.Empty,
            ValorTotal = 0,
            Status = OrderStatus.Criado,
            DataCriacao = DateTime.MinValue,
            DataAtualizacao = DateTime.MinValue,
            Itens = new List<OrderProduct>()
        };
    }

    /// <summary>
    /// Generates an order with zero total value.
    /// </summary>
    /// <returns>An Order with zero total value.</returns>
    public static Order GenerateOrderWithZeroTotal()
    {
        var order = orderFaker.Generate();
        order.ValorTotal = 0;
        return order;
    }

    /// <summary>
    /// Generates an order with negative total value.
    /// </summary>
    /// <returns>An Order with negative total value.</returns>
    public static Order GenerateOrderWithNegativeTotal()
    {
        var order = orderFaker.Generate();
        order.ValorTotal = -100.00m;
        return order;
    }

    /// <summary>
    /// Generates an order with high total value.
    /// </summary>
    /// <returns>An Order with high total value.</returns>
    public static Order GenerateOrderWithHighTotal()
    {
        var order = orderFaker.Generate();
        order.ValorTotal = 50000.00m;
        return order;
    }

    /// <summary>
    /// Generates an order with empty user ID.
    /// </summary>
    /// <returns>An Order with empty user ID.</returns>
    public static Order GenerateOrderWithEmptyUserId()
    {
        var order = orderFaker.Generate();
        order.UsuarioId = Guid.Empty;
        return order;
    }

    /// <summary>
    /// Generates an order with many items.
    /// </summary>
    /// <returns>An Order with many items.</returns>
    public static Order GenerateOrderWithManyItems()
    {
        var order = orderFaker.Generate();
        order.Itens = GenerateOrderItems(15); // More than 10 items
        return order;
    }

    /// <summary>
    /// Generates an order with no items.
    /// </summary>
    /// <returns>An Order with no items.</returns>
    public static Order GenerateOrderWithNoItems()
    {
        var order = orderFaker.Generate();
        order.Itens = new List<OrderProduct>();
        return order;
    }

    /// <summary>
    /// Generates a valid OrderProduct entity with randomized data.
    /// </summary>
    /// <returns>A valid OrderProduct entity with randomly generated data.</returns>
    public static OrderProduct GenerateValidOrderProduct()
    {
        return orderProductFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Order entities.
    /// </summary>
    /// <param name="count">Number of orders to generate.</param>
    /// <returns>A list of valid Order entities.</returns>
    public static List<Order> GenerateValidOrders(int count = 5)
    {
        return orderFaker.Generate(count);
    }

    /// <summary>
    /// Generates a list of valid OrderProduct entities.
    /// </summary>
    /// <param name="count">Number of order products to generate.</param>
    /// <returns>A list of valid OrderProduct entities.</returns>
    public static List<OrderProduct> GenerateValidOrderProducts(int count = 5)
    {
        return orderProductFaker.Generate(count);
    }
} 