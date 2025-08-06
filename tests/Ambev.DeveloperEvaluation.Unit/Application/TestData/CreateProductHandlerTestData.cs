using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Nome (product names)
    /// - Descricao (product descriptions)
    /// - Preco (positive decimal values)
    /// - Status (Ativo or Inativo)
    /// </summary>
    private static readonly Faker<CreateProductCommand> createProductCommandFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
        .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Preco, f => f.Random.Decimal(1.00m, 1000.00m))
        .RuleFor(p => p.Status, f => f.PickRandom(1, 2)); // 1 = Ativo, 2 = Inativo

    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// </summary>
    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
        .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Preco, f => f.Random.Decimal(1.00m, 1000.00m))
        .RuleFor(p => p.Status, f => f.PickRandom(ProductStatus.Ativo, ProductStatus.Inativo))
        .RuleFor(p => p.DataCriacao, f => f.Date.Past())
        .RuleFor(p => p.DataAtualizacao, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid CreateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductCommandFaker.Generate();
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
    /// Generates an invalid CreateProductCommand (empty or null values).
    /// </summary>
    /// <returns>An invalid CreateProductCommand.</returns>
    public static CreateProductCommand GenerateInvalidCommand()
    {
        return new CreateProductCommand
        {
            Nome = string.Empty,
            Descricao = string.Empty,
            Preco = -1.00m,
            Status = 1 // Use valid status instead of invalid one
        };
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