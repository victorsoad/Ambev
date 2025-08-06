using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Order entity class.
/// Tests cover basic entity properties and validation.
/// </summary>
public class OrderTests
{
    /// <summary>
    /// Tests that an order can be created with valid data.
    /// </summary>
    [Fact(DisplayName = "Order should be created with valid data")]
    public void Given_ValidOrderData_When_Created_Then_ShouldHaveCorrectProperties()
    {
        // Arrange
        var order = OrderTestData.GenerateValidOrder();

        // Act & Assert
        order.Should().NotBeNull();
        order.Id.Should().NotBeEmpty();
        order.UsuarioId.Should().NotBeEmpty();
        order.ValorTotal.Should().BeGreaterThanOrEqualTo(0);
        order.Status.Should().BeOneOf(OrderStatus.Criado, OrderStatus.Pago, OrderStatus.Cancelado);
        order.DataCriacao.Should().NotBe(default(DateTime));
    }

    /// <summary>
    /// Tests that an order with valid data has correct property values.
    /// </summary>
    [Fact(DisplayName = "Order should have correct property values")]
    public void Given_ValidOrder_When_Inspected_Then_PropertiesAreCorrect()
    {
        // Arrange
        var order = OrderTestData.GenerateValidOrder();

        // Act & Assert
        order.Should().NotBeNull();
        order.Id.Should().NotBeEmpty();
        order.UsuarioId.Should().NotBeEmpty();
        order.ValorTotal.Should().BeGreaterThanOrEqualTo(0);
        order.Status.Should().BeOneOf(OrderStatus.Criado, OrderStatus.Pago, OrderStatus.Cancelado);
    }

    /// <summary>
    /// Tests that an order with invalid data has incorrect property values.
    /// </summary>
    [Fact(DisplayName = "Order should handle invalid data correctly")]
    public void Given_InvalidOrder_When_Inspected_Then_PropertiesAreIncorrect()
    {
        // Arrange
        var order = OrderTestData.GenerateInvalidOrder();

        // Act & Assert
        order.Should().NotBeNull();
        order.Id.Should().BeEmpty();
        order.UsuarioId.Should().BeEmpty();
        order.ValorTotal.Should().Be(0);
    }

    /// <summary>
    /// Tests that an order with zero total value has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle zero total value correctly")]
    public void Given_OrderWithZeroTotal_When_Inspected_Then_TotalIsZero()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithZeroTotal();

        // Act & Assert
        order.Should().NotBeNull();
        order.ValorTotal.Should().Be(0);
    }

    /// <summary>
    /// Tests that an order with negative total value has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle negative total value correctly")]
    public void Given_OrderWithNegativeTotal_When_Inspected_Then_TotalIsNegative()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithNegativeTotal();

        // Act & Assert
        order.Should().NotBeNull();
        order.ValorTotal.Should().BeLessThan(0);
    }

    /// <summary>
    /// Tests that an order with high total value has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle high total value correctly")]
    public void Given_OrderWithHighTotal_When_Inspected_Then_TotalIsHigh()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithHighTotal();

        // Act & Assert
        order.Should().NotBeNull();
        order.ValorTotal.Should().BeGreaterThan(10000);
    }

    /// <summary>
    /// Tests that an order with empty user ID has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle empty user ID correctly")]
    public void Given_OrderWithEmptyUserId_When_Inspected_Then_UserIdIsEmpty()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithEmptyUserId();

        // Act & Assert
        order.Should().NotBeNull();
        order.UsuarioId.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that an order with many items has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle many items correctly")]
    public void Given_OrderWithManyItems_When_Inspected_Then_ItemsCountIsCorrect()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithManyItems();

        // Act & Assert
        order.Should().NotBeNull();
        order.Itens.Should().NotBeNull();
        order.Itens.Count.Should().BeGreaterThan(10);
    }

    /// <summary>
    /// Tests that an order with no items has the correct property.
    /// </summary>
    [Fact(DisplayName = "Order should handle no items correctly")]
    public void Given_OrderWithNoItems_When_Inspected_Then_ItemsCountIsZero()
    {
        // Arrange
        var order = OrderTestData.GenerateOrderWithNoItems();

        // Act & Assert
        order.Should().NotBeNull();
        order.Itens.Should().NotBeNull();
        order.Itens.Count.Should().Be(0);
    }

    /// <summary>
    /// Tests that orders can be compared correctly.
    /// </summary>
    [Fact(DisplayName = "Orders should be comparable")]
    public void Given_TwoOrders_When_Compared_Then_ComparisonWorks()
    {
        // Arrange
        var order1 = OrderTestData.GenerateValidOrder();
        var order2 = OrderTestData.GenerateValidOrder();

        // Act & Assert
        order1.Should().NotBeNull();
        order2.Should().NotBeNull();
        order1.Should().NotBe(order2);
    }

    /// <summary>
    /// Tests that an order has a non-empty ID.
    /// </summary>
    [Fact(DisplayName = "Order should have non-empty ID")]
    public void Given_ValidOrder_When_Inspected_Then_IdIsNotEmpty()
    {
        // Arrange
        var order = OrderTestData.GenerateValidOrder();

        // Act & Assert
        order.Should().NotBeNull();
        order.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that an order has a valid creation date.
    /// </summary>
    [Fact(DisplayName = "Order should have valid creation date")]
    public void Given_ValidOrder_When_Inspected_Then_CreationDateIsValid()
    {
        // Arrange
        var order = OrderTestData.GenerateValidOrder();

        // Act & Assert
        order.Should().NotBeNull();
        order.DataCriacao.Should().NotBe(default(DateTime));
    }
} 