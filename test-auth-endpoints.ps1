# Script para testar endpoints de autenticação, produtos e pedidos
$baseUrl = "https://localhost:7001"
$apiUrl = "$baseUrl/api"

Write-Host "🔐 Testando Endpoints de Autenticação e API" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green

# 1. Testar Login
Write-Host "`n1. Testando Login..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    Write-Host "✅ Login bem-sucedido!" -ForegroundColor Green
    Write-Host "   Username: $($loginResponse.data.username)" -ForegroundColor Cyan
    Write-Host "   Role: $($loginResponse.data.role)" -ForegroundColor Cyan
    Write-Host "   Token: $($loginResponse.data.token.Substring(0, 50))..." -ForegroundColor Cyan
    
    $token = $loginResponse.data.token
    $headers = @{
        "Authorization" = "Bearer $token"
        "Content-Type" = "application/json"
    }
} catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Testar Registro
Write-Host "`n2. Testando Registro..." -ForegroundColor Yellow
$registerBody = @{
    username = "testuser"
    email = "testuser@email.com"
    password = "senha123"
    confirmPassword = "senha123"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$apiUrl/auth/register" -Method POST -Body $registerBody -ContentType "application/json"
    Write-Host "✅ Registro bem-sucedido!" -ForegroundColor Green
    Write-Host "   Username: $($registerResponse.data.username)" -ForegroundColor Cyan
    Write-Host "   Email: $($registerResponse.data.email)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro no registro: $($_.Exception.Message)" -ForegroundColor Red
}

# 3. Testar Endpoints de Produtos
Write-Host "`n3. Testando Endpoints de Produtos..." -ForegroundColor Yellow

# 3.1 Listar produtos (GET)
try {
    $productsResponse = Invoke-RestMethod -Uri "$apiUrl/products" -Method GET -Headers $headers
    Write-Host "✅ GET /api/products - Sucesso!" -ForegroundColor Green
    Write-Host "   Produtos encontrados: $($productsResponse.data.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao listar produtos: $($_.Exception.Message)" -ForegroundColor Red
}

# 3.2 Buscar produto específico (GET)
try {
    $productId = "550e8400-e29b-41d4-a716-446655440000" # ID de exemplo
    $productResponse = Invoke-RestMethod -Uri "$apiUrl/products/$productId" -Method GET -Headers $headers
    Write-Host "✅ GET /api/products/{id} - Sucesso!" -ForegroundColor Green
    Write-Host "   Produto: $($productResponse.data.nome)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao buscar produto: $($_.Exception.Message)" -ForegroundColor Red
}

# 3.3 Criar produto (POST)
$newProductBody = @{
    nome = "Produto Teste"
    descricao = "Descrição do produto teste"
    preco = 15.99
} | ConvertTo-Json

try {
    $createProductResponse = Invoke-RestMethod -Uri "$apiUrl/products" -Method POST -Body $newProductBody -Headers $headers
    Write-Host "✅ POST /api/products - Sucesso!" -ForegroundColor Green
    Write-Host "   Produto criado: $($createProductResponse.data.nome)" -ForegroundColor Cyan
    Write-Host "   ID: $($createProductResponse.data.id)" -ForegroundColor Cyan
    
    $createdProductId = $createProductResponse.data.id
} catch {
    Write-Host "❌ Erro ao criar produto: $($_.Exception.Message)" -ForegroundColor Red
}

# 3.4 Atualizar produto (PUT)
if ($createdProductId) {
    $updateProductBody = @{
        nome = "Produto Teste Atualizado"
        descricao = "Descrição atualizada do produto teste"
        preco = 19.99
    } | ConvertTo-Json

    try {
        $updateProductResponse = Invoke-RestMethod -Uri "$apiUrl/products/$createdProductId" -Method PUT -Body $updateProductBody -Headers $headers
        Write-Host "✅ PUT /api/products/{id} - Sucesso!" -ForegroundColor Green
        Write-Host "   Produto atualizado: $($updateProductResponse.data.nome)" -ForegroundColor Cyan
    } catch {
        Write-Host "❌ Erro ao atualizar produto: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 4. Testar Endpoints de Pedidos
Write-Host "`n4. Testando Endpoints de Pedidos..." -ForegroundColor Yellow

# 4.1 Criar pedido (POST)
$newOrderBody = @{
    usuarioId = "550e8400-e29b-41d4-a716-446655440001"
    itens = @(
        @{
            productId = "550e8400-e29b-41d4-a716-446655440000"
            quantidade = 2
            precoUnitario = 8.50
        }
    )
} | ConvertTo-Json

try {
    $createOrderResponse = Invoke-RestMethod -Uri "$apiUrl/orders" -Method POST -Body $newOrderBody -Headers $headers
    Write-Host "✅ POST /api/orders - Sucesso!" -ForegroundColor Green
    Write-Host "   Pedido criado: ID $($createOrderResponse.data.id)" -ForegroundColor Cyan
    Write-Host "   Valor total: R$ $($createOrderResponse.data.valorTotal)" -ForegroundColor Cyan
    
    $createdOrderId = $createOrderResponse.data.id
} catch {
    Write-Host "❌ Erro ao criar pedido: $($_.Exception.Message)" -ForegroundColor Red
}

# 4.2 Buscar pedido específico (GET)
if ($createdOrderId) {
    try {
        $orderResponse = Invoke-RestMethod -Uri "$apiUrl/orders/$createdOrderId" -Method GET -Headers $headers
        Write-Host "✅ GET /api/orders/{id} - Sucesso!" -ForegroundColor Green
        Write-Host "   Pedido encontrado: ID $($orderResponse.data.id)" -ForegroundColor Cyan
        Write-Host "   Itens: $($orderResponse.data.itens.Count)" -ForegroundColor Cyan
    } catch {
        Write-Host "❌ Erro ao buscar pedido: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 4.3 Atualizar pedido (PUT)
if ($createdOrderId) {
    $updateOrderBody = @{
        status = 2 # Pago
    } | ConvertTo-Json

    try {
        $updateOrderResponse = Invoke-RestMethod -Uri "$apiUrl/orders/$createdOrderId" -Method PUT -Body $updateOrderBody -Headers $headers
        Write-Host "✅ PUT /api/orders/{id} - Sucesso!" -ForegroundColor Green
        Write-Host "   Pedido atualizado: Status $($updateOrderResponse.data.status)" -ForegroundColor Cyan
    } catch {
        Write-Host "❌ Erro ao atualizar pedido: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n🎉 Testes concluídos!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green 