using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
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
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return productFaker.Generate();
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

    /// <summary>
    /// Generates an invalid Product entity (empty or null values).
    /// </summary>
    /// <returns>An invalid Product entity.</returns>
    public static Product GenerateInvalidProduct()
    {
        return new Product
        {
            Id = Guid.Empty,
            Nome = string.Empty,
            Descricao = string.Empty,
            Preco = -1.00m,
            Status = (ProductStatus)999, // Invalid status
            DataCriacao = DateTime.MinValue,
            DataAtualizacao = DateTime.MinValue
        };
    }

    /// <summary>
    /// Generates a product with a very long name (exceeding limits).
    /// </summary>
    /// <returns>A Product with an excessively long name.</returns>
    public static Product GenerateProductWithLongName()
    {
        var product = productFaker.Generate();
        product.Nome = new string('A', 1000); // Very long name
        return product;
    }

    /// <summary>
    /// Generates a product with a very long description (exceeding limits).
    /// </summary>
    /// <returns>A Product with an excessively long description.</returns>
    public static Product GenerateProductWithLongDescription()
    {
        var product = productFaker.Generate();
        product.Descricao = new string('B', 2000); // Very long description
        return product;
    }

    /// <summary>
    /// Generates a product with zero price.
    /// </summary>
    /// <returns>A Product with zero price.</returns>
    public static Product GenerateProductWithZeroPrice()
    {
        var product = productFaker.Generate();
        product.Preco = 0.00m;
        return product;
    }

    /// <summary>
    /// Generates a product with negative price.
    /// </summary>
    /// <returns>A Product with negative price.</returns>
    public static Product GenerateProductWithNegativePrice()
    {
        var product = productFaker.Generate();
        product.Preco = -10.00m;
        return product;
    }

    /// <summary>
    /// Generates a product with very high price.
    /// </summary>
    /// <returns>A Product with very high price.</returns>
    public static Product GenerateProductWithHighPrice()
    {
        var product = productFaker.Generate();
        product.Preco = 999999.99m;
        return product;
    }

    /// <summary>
    /// Generates a product with null name.
    /// </summary>
    /// <returns>A Product with null name.</returns>
    public static Product GenerateProductWithNullName()
    {
        var product = productFaker.Generate();
        product.Nome = null!;
        return product;
    }

    /// <summary>
    /// Generates a product with null description.
    /// </summary>
    /// <returns>A Product with null description.</returns>
    public static Product GenerateProductWithNullDescription()
    {
        var product = productFaker.Generate();
        product.Descricao = null!;
        return product;
    }
} 