# 🚀 Parallel Programming in .NET Examples

📌 **.NET Version**: (especifique aqui)  
📜 **License**: MIT

Repositório com exemplos práticos de programação paralela em .NET, cobrindo:

- `Parallel.For`
- `Parallel.ForEach`
- `Parallel.Invoke`
- `Parallel.ForEachAsync` (C# 9+)
- Controle de paralelismo e cancelamento

---

## 📦 Exemplos Incluídos

### 1. Atualização em Massa de Pedidos

```csharp
Parallel.For(0, pedidos.Count, i => {
    pedidos[i].Status = "Enviado";
});
```

**Cenário**: Processamento paralelo de 1000 pedidos.

---

### 2. Cálculo de Frete Paralelo

```csharp
Parallel.ForEach(produtos, produto => {
    produto.Frete = CalcularFrete(produto.PesoKg);
});
```

**Cenário**: Cálculo simultâneo de fretes para 500 produtos.

---

### 3. Geração de Relatórios Simultâneos

```csharp
Parallel.Invoke(
    () => GerarRelatorioVendas(),
    () => GerarRelatorioEstoque()
);
```

**Cenário**: Geração paralela de múltiplos relatórios.

---

### 4. Busca Paralela com `ForEachAsync`

```csharp
await Parallel.ForEachAsync(produtosIds, async (id, ct) => {
    var produto = await BuscarProdutoApi(id);
});
```

**Cenário**: Consulta assíncrona paralela à API de produtos.

---

## 📊 Comparação de Métodos

| Método              | Melhor Para             | Threads         | Async Support |
|---------------------|--------------------------|------------------|----------------|
| `Parallel.For`      | Loops numéricos          | Múltiplas        | ❌             |
| `Parallel.ForEach`  | Coleções                 | Múltiplas        | ❌             |
| `Parallel.Invoke`   | Tarefas heterogêneas     | Múltiplas        | ❌             |
| `ForEachAsync`      | I/O-bound (C# 9+)        | Libera threads   | ✅             |

---

## ⚡️ Dicas de Performance

```csharp
// Limitar o grau de paralelismo para evitar sobrecarga
var options = new ParallelOptions { 
    MaxDegreeOfParallelism = 4 
};

// Cancelamento de operações
var cts = new CancellationTokenSource();

await Parallel.ForEachAsync(items, cts.Token, async (item, ct) => {
    await ProcessItemAsync(item, ct);
});
```

---

## 📚 Recursos Adicionais

- [📘 Documentação oficial da TPL (Task Parallel Library)](https://learn.microsoft.com/pt-br/dotnet/standard/parallel-programming/task-parallel-library-tpl)
- [📘 Paralelismo de dados com TPL](https://learn.microsoft.com/pt-br/dotnet/standard/parallel-programming/data-parallelism-task-parallel-library)
- [📘 Padrões de paralelismo em .NET](https://learn.microsoft.com/pt-br/dotnet/standard/parallel-programming/tpl-and-traditional-async-programming)
- [📘 Melhores práticas com async/await (MSDN Magazine)](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
- [📘 Boas práticas para programação assíncrona (.NET Q&A)](https://learn.microsoft.com/en-us/answers/questions/1375188/best-practices-for-asynchronous-programming-in-net)

---

## 📜 Licença

Este projeto está licenciado sob a licença [MIT](LICENSE) – veja o arquivo para mais detalhes.

---

<div align="center">
  <sub>Criado com ❤️ por <a href="https://github.com/alexandrenetit">Alexandre</a></sub>
</div>
