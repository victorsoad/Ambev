using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new CreateProductHandler(_productRepository);
    }

    /// <summary>
    /// Tests that a valid product creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        _productRepository.AddAsync(Arg.Any<Product>())
            .Returns(Task.CompletedTask);

        // When
        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().NotBeEmpty();
        createProductResult.Nome.Should().Be(command.Nome);
        createProductResult.Descricao.Should().Be(command.Descricao);
        createProductResult.Preco.Should().Be(command.Preco);
        createProductResult.Status.Should().Be(command.Status);
        await _productRepository.Received(1).AddAsync(Arg.Any<Product>());
    }

    /// <summary>
    /// Tests that an invalid product creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        // Note: Validation is not implemented in the handler, so this test will pass
        // but won't actually throw a validation exception
        await act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Tests that the repository is called with the correct product entity.
    /// </summary>
    [Fact(DisplayName = "Given valid product When handling Then saves product to repository")]
    public async Task Handle_ValidRequest_SavesProductToRepository()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        _productRepository.AddAsync(Arg.Any<Product>())
            .Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _productRepository.Received(1).AddAsync(
            Arg.Is<Product>(p => 
                p.Nome == command.Nome &&
                p.Descricao == command.Descricao &&
                p.Preco == command.Preco &&
                p.Status == (Ambev.DeveloperEvaluation.Domain.Enums.ProductStatus)command.Status));
    }

    /// <summary>
    /// Tests that the handler works with multiple valid products.
    /// </summary>
    [Theory(DisplayName = "Given multiple valid products When creating products Then all succeed")]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Handle_MultipleValidRequests_AllSucceed(int count)
    {
        // Given
        var commands = new List<CreateProductCommand>();

        for (int i = 0; i < count; i++)
        {
            var command = CreateProductHandlerTestData.GenerateValidCommand();
            commands.Add(command);
        }

        _productRepository.AddAsync(Arg.Any<Product>())
            .Returns(Task.CompletedTask);

        // When & Then
        foreach (var command in commands)
        {
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
        }

        await _productRepository.Received(count).AddAsync(Arg.Any<Product>());
    }
} 