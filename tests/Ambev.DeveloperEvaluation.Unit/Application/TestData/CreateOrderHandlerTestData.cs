using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Provides methods for generating test data for Order entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateOrderHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateOrderCommand entities.
    /// </summary>
    private static readonly Faker<CreateOrderCommand> createOrderCommandFaker = new Faker<CreateOrderCommand>()
        .RuleFor(o => o.UsuarioId, f => f.Random.Guid())
        .RuleFor(o => o.Itens, f => GenerateOrderItems(f.Random.Number(1, 5)));

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
        .RuleFor(o => o.Itens, f => new List<OrderProduct>()); // Initialize with empty list

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
    /// Configures the Faker to generate valid Product entities.
    /// </summary>
    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
        .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Preco, f => f.Random.Decimal(1.00m, 100.00m))
        .RuleFor(p => p.Status, f => f.PickRandom(ProductStatus.Ativo, ProductStatus.Inativo))
        .RuleFor(p => p.DataCriacao, f => f.Date.Past());

    /// <summary>
    /// Generates a list of order items for testing.
    /// </summary>
    /// <param name="count">Number of items to generate.</param>
    /// <returns>A list of OrderItemDto.</returns>
    private static List<OrderItemDto> GenerateOrderItems(int count)
    {
        var items = new List<OrderItemDto>();
        for (int i = 0; i < count; i++)
        {
            items.Add(new OrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantidade = new Faker().Random.Number(1, 10)
            });
        }
        return items;
    }

    /// <summary>
    /// Generates a valid CreateOrderCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateOrderCommand with randomly generated data.</returns>
    public static CreateOrderCommand GenerateValidCommand()
    {
        return createOrderCommandFaker.Generate();
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
    /// Generates a valid OrderProduct entity with randomized data.
    /// </summary>
    /// <returns>A valid OrderProduct entity with randomly generated data.</returns>
    public static OrderProduct GenerateValidOrderProduct()
    {
        return orderProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return productFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid CreateOrderCommand (empty or null values).
    /// </summary>
    /// <returns>An invalid CreateOrderCommand.</returns>
    public static CreateOrderCommand GenerateInvalidCommand()
    {
        return new CreateOrderCommand
        {
            UsuarioId = Guid.Empty,
            Itens = new List<OrderItemDto>() // Empty items list
        };
    }

    /// <summary>
    /// Generates a command with too many items (exceeding the 20 item limit).
    /// </summary>
    /// <returns>A CreateOrderCommand with too many items.</returns>
    public static CreateOrderCommand GenerateCommandWithTooManyItems()
    {
        var items = new List<OrderItemDto>();
        for (int i = 0; i < 25; i++) // More than 20 items
        {
            items.Add(new OrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantidade = 1
            });
        }

        return new CreateOrderCommand
        {
            UsuarioId = Guid.NewGuid(),
            Itens = items
        };
    }

    /// <summary>
    /// Generates a command with items exceeding the 20 quantity limit.
    /// </summary>
    /// <returns>A CreateOrderCommand with items exceeding quantity limit.</returns>
    public static CreateOrderCommand GenerateCommandWithExcessiveQuantity()
    {
        return new CreateOrderCommand
        {
            UsuarioId = Guid.NewGuid(),
            Itens = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantidade = 25 // More than 20
                }
            }
        };
    }

    /// <summary>
    /// Generates a command that should trigger 20% discount (10+ items and >100 total).
    /// </summary>
    /// <returns>A CreateOrderCommand that triggers 20% discount.</returns>
    public static CreateOrderCommand GenerateCommandFor20PercentDiscount()
    {
        var items = new List<OrderItemDto>();
        for (int i = 0; i < 12; i++) // 12 items
        {
            items.Add(new OrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantidade = 1
            });
        }

        return new CreateOrderCommand
        {
            UsuarioId = Guid.NewGuid(),
            Itens = items
        };
    }

    /// <summary>
    /// Generates a command that should trigger 10% discount (5+ items or >50 total).
    /// </summary>
    /// <returns>A CreateOrderCommand that triggers 10% discount.</returns>
    public static CreateOrderCommand GenerateCommandFor10PercentDiscount()
    {
        var items = new List<OrderItemDto>();
        for (int i = 0; i < 6; i++) // 6 items
        {
            items.Add(new OrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantidade = 1
            });
        }

        return new CreateOrderCommand
        {
            UsuarioId = Guid.NewGuid(),
            Itens = items
        };
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
    /// Generates a list of valid Product entities.
    /// </summary>
    /// <param name="count">Number of products to generate.</param>
    /// <returns>A list of valid Product entities.</returns>
    public static List<Product> GenerateValidProducts(int count = 5)
    {
        return productFaker.Generate(count);
    }
} 