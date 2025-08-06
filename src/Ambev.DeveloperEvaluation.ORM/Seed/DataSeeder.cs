using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(DefaultContext context)
    {
        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new Product
                {
                    Nome = "Cerveja Heineken",
                    Descricao = "Cerveja premium importada holandesa",
                    Preco = 8.50m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Stella Artois",
                    Descricao = "Cerveja belga premium",
                    Preco = 7.80m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Budweiser",
                    Descricao = "Cerveja americana tradicional",
                    Preco = 6.90m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Corona",
                    Descricao = "Cerveja mexicana com limão",
                    Preco = 9.20m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Brahma",
                    Descricao = "Cerveja brasileira tradicional",
                    Preco = 5.50m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Skol",
                    Descricao = "Cerveja brasileira popular",
                    Preco = 5.20m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Antarctica",
                    Descricao = "Cerveja brasileira clássica",
                    Preco = 5.80m,
                    Status = ProductStatus.Ativo
                },
                new Product
                {
                    Nome = "Cerveja Bohemia",
                    Descricao = "Cerveja brasileira premium",
                    Preco = 7.50m,
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
                        ValorTotal = 25.60m,
                        Status = OrderStatus.Criado,
                        Itens = new List<OrderProduct>
                        {
                            new OrderProduct
                            {
                                ProductId = products[0].Id,
                                Quantidade = 3,
                                PrecoUnitario = 8.50m
                            }
                        }
                    },
                    new Order
                    {
                        UsuarioId = Guid.NewGuid(), // Mock user ID
                        ValorTotal = 45.20m,
                        Status = OrderStatus.Pago,
                        Itens = new List<OrderProduct>
                        {
                            new OrderProduct
                            {
                                ProductId = products[1].Id,
                                Quantidade = 2,
                                PrecoUnitario = 7.80m
                            },
                            new OrderProduct
                            {
                                ProductId = products[2].Id,
                                Quantidade = 4,
                                PrecoUnitario = 6.90m
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