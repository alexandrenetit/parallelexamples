# 🚀 Parallel Programming in .NET - E-commerce Examples

![.NET Version](https://img.shields.io/badge/.NET-%3E%3D6.0-blue)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Repositório com exemplos práticos de programação paralela em .NET para cenários de e-commerce, cobrindo:

- `Parallel.For`
- `Parallel.ForEach`
- `Parallel.Invoke`
- `Parallel.ForEachAsync` (C# 9+)
- Controle de paralelismo e cancelamento

## 📦 Exemplos Incluídos

### 1. Atualização em Massa de Pedidos
```csharp
Parallel.For(0, pedidos.Count, i => {
    pedidos[i].Status = "Enviado";
});
Cenário: Processamento paralelo de 1000 pedidos.

2. Cálculo de Frete Paralelo
csharp
Parallel.ForEach(produtos, produto => {
    produto.Frete = CalcularFrete(produto.PesoKg);
});
Cenário: Cálculo simultâneo de fretes para 500 produtos.

3. Geração de Relatórios Simultâneos
csharp
Parallel.Invoke(
    () => GerarRelatorioVendas(),
    () => GerarRelatorioEstoque()
);
Cenário: Geração paralela de múltiplos relatórios.

4. Busca Paralela com ForEachAsync
csharp
await Parallel.ForEachAsync(produtosIds, async (id, ct) => {
    var produto = await BuscarProdutoApi(id);
});
Cenário: Consulta assíncrona paralela a API de produtos.

🛠️ Como Executar
Clone o repositório:

bash
git clone https://github.com/seu-usuario/parallel-ecommerce-dotnet.git
Execute qualquer exemplo:

bash
cd src/ParallelPedidos
dotnet run
📊 Comparação de Métodos
Método	Melhor Para	Threads	Adequado para Async
Parallel.For	Loops numéricos	Múltiplas	❌
Parallel.ForEach	Coleções	Múltiplas	❌
Parallel.Invoke	Tarefas heterogêneas	Múltiplas	❌
ForEachAsync	I/O-bound	Libera threads	✅
🚀 Performance Tips
csharp
// Limite o paralelismo para APIs externas
var options = new ParallelOptions { 
    MaxDegreeOfParallelism = 4 
};

// Use cancellation tokens
var cts = new CancellationTokenSource();
Parallel.ForEachAsync(items, cts.Token, async (item, ct) => {
    await ProcessItemAsync(item, ct);
});
📚 Recursos Adicionais
Documentação oficial da TPL

Padrões de paralelismo em .NET

Async/Await Best Practices

🤝 Contribuição
Contribuições são bem-vindas! Siga estes passos:

Fork o projeto

Crie sua branch (git checkout -b feature/improvement)

Commit suas mudanças (git commit -m 'Add new feature')

Push para a branch (git push origin feature/improvement)

Abra um Pull Request

📜 Licença
Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.

<div align="center"> <sub>Criado com ❤️ por <a href="https://github.com/alexandrenetit">Alexandre</a></sub> </div> ```