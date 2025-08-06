using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(DefaultContext context)
    {
        // Seed Users
        if (!await context.Users.AnyAsync())
        {
            var passwordHasher = new BCryptPasswordHasher();
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@ambev.com",
                    PasswordHash = passwordHasher.HashPassword("admin123"),
                    Role = "Admin",
                    IsActive = true,
                    LastLogin = DateTime.UtcNow
                },
                new User
                {
                    Username = "user1",
                    Email = "user1@ambev.com",
                    PasswordHash = passwordHasher.HashPassword("user123"),
                    Role = "User",
                    IsActive = true,
                    LastLogin = DateTime.UtcNow
                },
                new User
                {
                    Username = "user2",
                    Email = "user2@ambev.com",
                    PasswordHash = passwordHasher.HashPassword("user123"),
                    Role = "User",
                    IsActive = true,
                    LastLogin = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new Product
                {
                    Nome = "Caneta Bic Azul",
                    Descricao = "Caneta esferográfica azul de ponta média",
                    Preco = 2.50m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Lápis Faber-Castell",
                    Descricao = "Lápis grafite HB para desenho e escrita",
                    Preco = 1.80m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Caderno Espiral 96 folhas",
                    Descricao = "Caderno universitário com espiral e capa dura",
                    Preco = 8.90m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Borracha Mercur",
                    Descricao = "Borracha branca macia para lápis",
                    Preco = 1.20m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Régua 30cm",
                    Descricao = "Régua plástica transparente de 30 centímetros",
                    Preco = 3.50m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Tesoura Escolar",
                    Descricao = "Tesoura com ponta arredondada para uso escolar",
                    Preco = 4.20m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cola Branca 90ml",
                    Descricao = "Cola branca escolar para papel e cartolina",
                    Preco = 2.80m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Marca-texto Amarelo",
                    Descricao = "Marca-texto fluorescente amarelo",
                    Preco = 3.50m,
                    Status = ProductStatus.Ativo
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Seed Orders (with sample order items)
        if (!await context.Orders.AnyAsync())
        {
            var products = await context.Products.ToListAsync();

            if (products.Any())
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        UsuarioId = Guid.NewGuid(), // Mock user ID
                        ValorTotal = 15.60m,
                        Status = OrderStatus.Criado,
                        Itens = new List<OrderProduct>
                        {
                            new OrderProduct
                            {
                                ProductId = products[0].Id,
                                Quantidade = 3,
                                PrecoUnitario = 2.50m
                            },
                            new OrderProduct
                            {
                                ProductId = products[1].Id,
                                Quantidade = 5,
                                PrecoUnitario = 1.80m
                            }
                        }
                    },
                    new Order
                    {
                        UsuarioId = Guid.NewGuid(), // Mock user ID
                        ValorTotal = 28.40m,
                        Status = OrderStatus.Pago,
                        Itens = new List<OrderProduct>
                        {
                            new OrderProduct
                            {
                                ProductId = products[2].Id,
                                Quantidade = 2,
                                PrecoUnitario = 8.90m
                            },
                            new OrderProduct
                            {
                                ProductId = products[3].Id,
                                Quantidade = 4,
                                PrecoUnitario = 1.20m
                            },
                            new OrderProduct
                            {
                                ProductId = products[4].Id,
                                Quantidade = 1,
                                PrecoUnitario = 3.50m
                            }
                        }
                    }
                };

                await context.Orders.AddRangeAsync(orders);
                await context.SaveChangesAsync();
            }
        }
    }
} 