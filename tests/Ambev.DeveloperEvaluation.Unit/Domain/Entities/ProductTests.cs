using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover basic entity properties and validation.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that a product can be created with valid data.
    /// </summary>
    [Fact(DisplayName = "Product should be created with valid data")]
    public void Given_ValidProductData_When_Created_Then_ShouldHaveCorrectProperties()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Nome.Should().NotBeNullOrEmpty();
        product.Descricao.Should().NotBeNullOrEmpty();
        product.Preco.Should().BeGreaterThan(0);
        product.Status.Should().BeOneOf(ProductStatus.Ativo, ProductStatus.Inativo);
        product.DataCriacao.Should().NotBe(default(DateTime));
    }

    /// <summary>
    /// Tests that a product with valid data has correct property values.
    /// </summary>
    [Fact(DisplayName = "Product should have correct property values")]
    public void Given_ValidProduct_When_Inspected_Then_PropertiesAreCorrect()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Nome.Should().NotBeNullOrEmpty();
        product.Descricao.Should().NotBeNullOrEmpty();
        product.Preco.Should().BeGreaterThan(0);
        product.Status.Should().BeOneOf(ProductStatus.Ativo, ProductStatus.Inativo);
    }

    /// <summary>
    /// Tests that a product with invalid data has incorrect property values.
    /// </summary>
    [Fact(DisplayName = "Product should handle invalid data correctly")]
    public void Given_InvalidProduct_When_Inspected_Then_PropertiesAreIncorrect()
    {
        // Arrange
        var product = ProductTestData.GenerateInvalidProduct();

        // Act & Assert
        product.Should().NotBeNull();
        product.Id.Should().BeEmpty();
        product.Nome.Should().BeEmpty();
        product.Descricao.Should().BeEmpty();
        product.Preco.Should().BeLessThan(0);
    }

    /// <summary>
    /// Tests that a product with a very long name has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle long name correctly")]
    public void Given_ProductWithLongName_When_Inspected_Then_NameIsCorrect()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithLongName();

        // Act & Assert
        product.Should().NotBeNull();
        product.Nome.Should().NotBeNullOrEmpty();
        product.Nome.Length.Should().BeGreaterThan(100);
    }

    /// <summary>
    /// Tests that a product with a very long description has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle long description correctly")]
    public void Given_ProductWithLongDescription_When_Inspected_Then_DescriptionIsCorrect()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithLongDescription();

        // Act & Assert
        product.Should().NotBeNull();
        product.Descricao.Should().NotBeNullOrEmpty();
        product.Descricao.Length.Should().BeGreaterThan(100);
    }

    /// <summary>
    /// Tests that a product with zero price has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle zero price correctly")]
    public void Given_ProductWithZeroPrice_When_Inspected_Then_PriceIsZero()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithZeroPrice();

        // Act & Assert
        product.Should().NotBeNull();
        product.Preco.Should().Be(0);
    }

    /// <summary>
    /// Tests that a product with negative price has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle negative price correctly")]
    public void Given_ProductWithNegativePrice_When_Inspected_Then_PriceIsNegative()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithNegativePrice();

        // Act & Assert
        product.Should().NotBeNull();
        product.Preco.Should().BeLessThan(0);
    }

    /// <summary>
    /// Tests that a product with very high price has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle high price correctly")]
    public void Given_ProductWithHighPrice_When_Inspected_Then_PriceIsHigh()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithHighPrice();

        // Act & Assert
        product.Should().NotBeNull();
        product.Preco.Should().BeGreaterThan(100000);
    }

    /// <summary>
    /// Tests that a product with null name has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle null name correctly")]
    public void Given_ProductWithNullName_When_Inspected_Then_NameIsNull()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithNullName();

        // Act & Assert
        product.Should().NotBeNull();
        product.Nome.Should().BeNull();
    }

    /// <summary>
    /// Tests that a product with null description has the correct property.
    /// </summary>
    [Fact(DisplayName = "Product should handle null description correctly")]
    public void Given_ProductWithNullDescription_When_Inspected_Then_DescriptionIsNull()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithNullDescription();

        // Act & Assert
        product.Should().NotBeNull();
        product.Descricao.Should().BeNull();
    }

    /// <summary>
    /// Tests that products can be compared correctly.
    /// </summary>
    [Fact(DisplayName = "Products should be comparable")]
    public void Given_TwoProducts_When_Compared_Then_ComparisonWorks()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product1.Should().NotBeNull();
        product2.Should().NotBeNull();
        product1.Should().NotBe(product2);
    }

    /// <summary>
    /// Tests that a product has a non-empty ID.
    /// </summary>
    [Fact(DisplayName = "Product should have non-empty ID")]
    public void Given_ValidProduct_When_Inspected_Then_IdIsNotEmpty()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that a product has a valid creation date.
    /// </summary>
    [Fact(DisplayName = "Product should have valid creation date")]
    public void Given_ValidProduct_When_Inspected_Then_CreationDateIsValid()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product.Should().NotBeNull();
        product.DataCriacao.Should().NotBe(default(DateTime));
    }
} 