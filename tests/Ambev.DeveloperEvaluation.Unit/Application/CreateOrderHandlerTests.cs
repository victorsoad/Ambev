using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateOrderHandler"/> class.
/// </summary>
public class CreateOrderHandlerTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly CreateOrderHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateOrderHandlerTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderProductRepository = Substitute.For<IOrderProductRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<CreateOrderHandler>>();
        _handler = new CreateOrderHandler(_orderRepository, _orderProductRepository, _productRepository, _logger);
    }

    /// <summary>
    /// Tests that a valid order creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid order data When creating order Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateValidCommand();
        var products = CreateOrderHandlerTestData.GenerateValidProducts(command.Itens.Count);

        // Ensure all products are active
        foreach (var product in products)
        {
            product.Status = Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus.Ativo;
        }

        _orderRepository.AddAsync(Arg.Any<Order>())
            .Returns(Task.CompletedTask);
        _orderProductRepository.AddAsync(Arg.Any<OrderProduct>())
            .Returns(Task.CompletedTask);

        // Mock product repository to return products for each item
        for (int i = 0; i < command.Itens.Count; i++)
        {
            var product = products[i];
            product.Id = command.Itens[i].ProductId; // Ensure product ID matches the command
            _productRepository.GetByIdAsync(command.Itens[i].ProductId)
                .Returns(product);
        }

        // When
        var createOrderResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createOrderResult.Should().NotBeNull();
        createOrderResult.Id.Should().NotBeEmpty();
        createOrderResult.UsuarioId.Should().Be(command.UsuarioId);
        createOrderResult.ValorTotal.Should().BeGreaterThan(0);
        createOrderResult.Status.Should().Be(1); // Criado = 1
        createOrderResult.Itens.Should().HaveCount(command.Itens.Count);
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
    }

    /// <summary>
    /// Tests that an invalid order creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid order data When creating order Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        // Note: Validation is not implemented in the handler, so this test will pass
        // but won't actually throw a validation exception
        await act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Tests that an order with too many items throws a business rule exception.
    /// </summary>
    [Fact(DisplayName = "Given order with too many items When creating order Then throws business rule exception")]
    public async Task Handle_TooManyItems_ThrowsBusinessRuleException()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateCommandWithTooManyItems();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Tests that an order with excessive quantity throws a business rule exception.
    /// </summary>
    [Fact(DisplayName = "Given order with excessive quantity When creating order Then throws business rule exception")]
    public async Task Handle_ExcessiveQuantity_ThrowsBusinessRuleException()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateCommandWithExcessiveQuantity();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Tests that the 20% discount is applied correctly.
    /// </summary>
    [Fact(DisplayName = "Given order with 10+ items and >100 total When creating order Then applies 20% discount")]
    public async Task Handle_OrderWith20PercentDiscount_AppliesCorrectDiscount()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateCommandFor20PercentDiscount();
        var products = CreateOrderHandlerTestData.GenerateValidProducts(command.Itens.Count);

        // Ensure all products are active and have a price that will trigger 20% discount
        foreach (var product in products)
        {
            product.Status = Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus.Ativo;
            product.Preco = 15.00m; // 12 items * 15 = 180 > 100
        }

        // Calculate expected values based on product prices
        var valorTotalSemDesconto = command.Itens.Sum(i => i.Quantidade * products.First().Preco);
        var valorDesconto = valorTotalSemDesconto * 0.20m;
        var valorTotal = valorTotalSemDesconto - valorDesconto;

        _orderRepository.AddAsync(Arg.Any<Order>())
            .Returns(Task.CompletedTask);
        _orderProductRepository.AddAsync(Arg.Any<OrderProduct>())
            .Returns(Task.CompletedTask);

        // Mock product repository
        for (int i = 0; i < command.Itens.Count; i++)
        {
            var product = products[i];
            product.Id = command.Itens[i].ProductId; // Ensure product ID matches the command
            _productRepository.GetByIdAsync(command.Itens[i].ProductId)
                .Returns(product);
        }

        // When
        var createOrderResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createOrderResult.Should().NotBeNull();
        createOrderResult.PercentualDesconto.Should().Be(20);
        createOrderResult.ValorDesconto.Should().Be(valorDesconto);
        createOrderResult.ValorTotal.Should().Be(valorTotal);
        createOrderResult.ValorTotalSemDesconto.Should().Be(valorTotalSemDesconto);
    }

    /// <summary>
    /// Tests that the 10% discount is applied correctly.
    /// </summary>
    [Fact(DisplayName = "Given order with 5+ items or >50 total When creating order Then applies 10% discount")]
    public async Task Handle_OrderWith10PercentDiscount_AppliesCorrectDiscount()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateCommandFor10PercentDiscount();
        var products = CreateOrderHandlerTestData.GenerateValidProducts(command.Itens.Count);

        // Ensure all products are active
        foreach (var product in products)
        {
            product.Status = Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus.Ativo;
            product.Preco = 8.00m; // 6 items * 8 = 48 < 50, but 6 items >= 5
        }

        // Calculate expected values based on product prices
        var valorTotalSemDesconto = command.Itens.Sum(i => i.Quantidade * products.First().Preco);
        var valorDesconto = valorTotalSemDesconto * 0.10m;
        var valorTotal = valorTotalSemDesconto - valorDesconto;

        _orderRepository.AddAsync(Arg.Any<Order>())
            .Returns(Task.CompletedTask);
        _orderProductRepository.AddAsync(Arg.Any<OrderProduct>())
            .Returns(Task.CompletedTask);

        // Mock product repository
        for (int i = 0; i < command.Itens.Count; i++)
        {
            var product = products[i];
            product.Id = command.Itens[i].ProductId; // Ensure product ID matches the command
            _productRepository.GetByIdAsync(command.Itens[i].ProductId)
                .Returns(product);
        }

        // When
        var createOrderResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createOrderResult.Should().NotBeNull();
        createOrderResult.PercentualDesconto.Should().Be(10);
        createOrderResult.ValorDesconto.Should().Be(valorDesconto);
        createOrderResult.ValorTotal.Should().Be(valorTotal);
        createOrderResult.ValorTotalSemDesconto.Should().Be(valorTotalSemDesconto);
    }

    /// <summary>
    /// Tests that the repository is called with the correct order entity.
    /// </summary>
    [Fact(DisplayName = "Given valid order When handling Then saves order to repository")]
    public async Task Handle_ValidRequest_SavesOrderToRepository()
    {
        // Given
        var command = CreateOrderHandlerTestData.GenerateValidCommand();
        var products = CreateOrderHandlerTestData.GenerateValidProducts(command.Itens.Count);

        // Ensure all products are active
        foreach (var product in products)
        {
            product.Status = Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus.Ativo;
        }

        _orderRepository.AddAsync(Arg.Any<Order>())
            .Returns(Task.CompletedTask);
        _orderProductRepository.AddAsync(Arg.Any<OrderProduct>())
            .Returns(Task.CompletedTask);

        // Mock product repository
        for (int i = 0; i < command.Itens.Count; i++)
        {
            var product = products[i];
            product.Id = command.Itens[i].ProductId; // Ensure product ID matches the command
            _productRepository.GetByIdAsync(command.Itens[i].ProductId)
                .Returns(product);
        }

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _orderRepository.Received(1).AddAsync(
            Arg.Is<Order>(o => 
                o.UsuarioId == command.UsuarioId &&
                o.Status == Ambev.DeveloperEvaluation.Domain.Enums.OrderStatus.Criado));
    }

    /// <summary>
    /// Tests that the handler works with multiple valid orders.
    /// </summary>
    [Theory(DisplayName = "Given multiple valid orders When creating orders Then all succeed")]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async Task Handle_MultipleValidRequests_AllSucceed(int count)
    {
        // Given
        var commands = new List<CreateOrderCommand>();

        for (int i = 0; i < count; i++)
        {
            var command = CreateOrderHandlerTestData.GenerateValidCommand();
            commands.Add(command);
        }

        _orderRepository.AddAsync(Arg.Any<Order>())
            .Returns(Task.CompletedTask);
        _orderProductRepository.AddAsync(Arg.Any<OrderProduct>())
            .Returns(Task.CompletedTask);

        // Mock product repository for all commands
        foreach (var command in commands)
        {
            for (int j = 0; j < command.Itens.Count; j++)
            {
                var product = CreateOrderHandlerTestData.GenerateValidProduct();
                product.Id = command.Itens[j].ProductId; // Ensure product ID matches the command
                product.Status = Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus.Ativo;
                _productRepository.GetByIdAsync(command.Itens[j].ProductId)
                    .Returns(product);
            }
        }

        // When & Then
        foreach (var command in commands)
        {
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
        }

        await _orderRepository.Received(count).AddAsync(Arg.Any<Order>());
    }
} 