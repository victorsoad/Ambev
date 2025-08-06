# Script para testar a API com produtos de material escolar e pedidos
# Autor: Assistant
# Data: 2025-08-05

Write-Host "=== Teste da API - Produtos de Material Escolar ===" -ForegroundColor Green

# Array de produtos de material escolar
$produtos = @(
    @{
        nome = "Caderno 96 folhas"
        descricao = "Caderno espiral 96 folhas pautadas, capa dura"
        preco = 12.50
        status = 1
    },
    @{
        nome = "Lápis HB"
        descricao = "Caixa com 12 lápis HB grafite"
        preco = 8.90
        status = 1
    },
    @{
        nome = "Caneta Azul"
        descricao = "Pacote com 10 canetas esferográficas azuis"
        preco = 15.80
        status = 1
    },
    @{
        nome = "Borracha"
        descricao = "Borracha branca escolar, tamanho médio"
        preco = 2.50
        status = 1
    },
    @{
        nome = "Régua 30cm"
        descricao = "Régua plástica transparente 30cm"
        preco = 4.20
        status = 1
    },
    @{
        nome = "Tesoura Escolar"
        descricao = "Tesoura escolar com pontas arredondadas"
        preco = 6.75
        status = 1
    },
    @{
        nome = "Cola Branca"
        descricao = "Cola branca escolar 90ml"
        preco = 3.90
        status = 1
    },
    @{
        nome = "Mochila Escolar"
        descricao = "Mochila escolar com alças ajustáveis"
        preco = 45.00
        status = 1
    },
    @{
        nome = "Estojo"
        descricao = "Estojo escolar com zíper, múltiplos compartimentos"
        preco = 18.50
        status = 1
    },
    @{
        nome = "Lápis de Cor"
        descricao = "Caixa com 24 lápis de cor"
        preco = 22.80
        status = 1
    }
)

# Criar produtos
Write-Host "`nCriando produtos de material escolar..." -ForegroundColor Yellow
$produtosIds = @()

foreach ($produto in $produtos) {
    try {
        $body = $produto | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "http://localhost:5119/api/products" -Method POST -Body $body -ContentType "application/json"
        
        if ($response.success) {
            Write-Host "✅ Produto criado: $($produto.nome)" -ForegroundColor Green
            $produtosIds += $response.data.id
        } else {
            Write-Host "❌ Erro ao criar produto: $($produto.nome)" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "❌ Erro na requisição: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== Teste da API - Pedidos ===" -ForegroundColor Green

# Array de pedidos de exemplo
$pedidos = @(
    @{
        usuarioId = "00000000-0000-0000-0000-000000000001"
        itens = @(
            @{
                productId = $produtosIds[0]  # Caderno
                quantidade = 3
                precoUnitario = 12.50
            },
            @{
                productId = $produtosIds[1]  # Lápis
                quantidade = 2
                precoUnitario = 8.90
            },
            @{
                productId = $produtosIds[2]  # Caneta
                quantidade = 1
                precoUnitario = 15.80
            }
        )
    },
    @{
        usuarioId = "00000000-0000-0000-0000-000000000002"
        itens = @(
            @{
                productId = $produtosIds[7]  # Mochila
                quantidade = 1
                precoUnitario = 45.00
            },
            @{
                productId = $produtosIds[8]  # Estojo
                quantidade = 1
                precoUnitario = 18.50
            },
            @{
                productId = $produtosIds[9]  # Lápis de Cor
                quantidade = 2
                precoUnitario = 22.80
            }
        )
    },
    @{
        usuarioId = "00000000-0000-0000-0000-000000000001"
        itens = @(
            @{
                productId = $produtosIds[3]  # Borracha
                quantidade = 5
                precoUnitario = 2.50
            },
            @{
                productId = $produtosIds[4]  # Régua
                quantidade = 3
                precoUnitario = 4.20
            },
            @{
                productId = $produtosIds[5]  # Tesoura
                quantidade = 2
                precoUnitario = 6.75
            },
            @{
                productId = $produtosIds[6]  # Cola
                quantidade = 4
                precoUnitario = 3.90
            }
        )
    }
)

# Criar pedidos
Write-Host "`nCriando pedidos..." -ForegroundColor Yellow

foreach ($pedido in $pedidos) {
    try {
        $body = $pedido | ConvertTo-Json -Depth 3
        $response = Invoke-RestMethod -Uri "http://localhost:5119/api/orders" -Method POST -Body $body -ContentType "application/json"
        
        if ($response.success) {
            Write-Host "✅ Pedido criado com sucesso!" -ForegroundColor Green
            Write-Host "   ID: $($response.data.id)" -ForegroundColor Cyan
            Write-Host "   Valor Total: R$ $($response.data.valorTotal)" -ForegroundColor Cyan
            Write-Host "   Valor Sem Desconto: R$ $($response.data.valorTotalSemDesconto)" -ForegroundColor Cyan
            Write-Host "   Desconto: R$ $($response.data.valorDesconto) ($($response.data.percentualDesconto)%)" -ForegroundColor Cyan
        } else {
            Write-Host "❌ Erro ao criar pedido" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "❌ Erro na requisição: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== Teste Concluído ===" -ForegroundColor Green
Write-Host "Acesse o Swagger em: http://localhost:5119/swagger" -ForegroundColor Cyan 