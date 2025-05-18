using Bogus;
using System.Collections.Concurrent;

internal class Program
{
    private static async Task Main()
    {
        // Configuração dos fakers
        var produtoFaker = new Faker<Produto>("pt_BR")
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
            .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Preco, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.Categoria, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Estoque, f => f.Random.Int(0, 100));

        var clienteFaker = new Faker<Cliente>("pt_BR")
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"))
            .RuleFor(c => c.Endereco, f => new Endereco
            {
                Rua = f.Address.StreetName(),
                Numero = f.Address.BuildingNumber(),
                Cidade = f.Address.City(),
                Estado = f.Address.StateAbbr(),
                CEP = f.Address.ZipCode("#####-###")
            });

        // Gerar dados iniciais
        var produtos = produtoFaker.Generate(50);
        var clientes = clienteFaker.Generate(20);

        // Gerar 100 pedidos fictícios
        var pedidos = new Faker<Pedido>("pt_BR")
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Cliente, f => f.PickRandom(clientes))
            .RuleFor(p => p.Data, f => f.Date.Recent(7))
            .RuleFor(p => p.Itens, f =>
            {
                var itens = new List<ItemPedido>();
                var qtdItens = f.Random.Int(1, 5);
                for (int i = 0; i < qtdItens; i++)
                {
                    var produto = f.PickRandom(produtos);
                    itens.Add(new ItemPedido
                    {
                        Produto = produto,
                        Quantidade = f.Random.Int(1, 3),
                        PrecoUnitario = produto.Preco
                    });
                }
                return itens;
            })
            .RuleFor(p => p.Status, f => StatusPedido.Pendente)
            .Generate(100);

        // Configuração do processamento paralelo
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        Console.WriteLine("Sistema de Processamento de Pedidos - E-commerce");
        Console.WriteLine($"Total de pedidos: {pedidos.Count}");
        Console.WriteLine("Pressione ENTER para cancelar...\n");

        // Iniciar monitoramento do cancelamento
        var monitorCancelamento = Task.Run(() =>
        {
            Console.ReadLine();
            Console.WriteLine("\nSolicitação de cancelamento recebida...");
            cts.Cancel();
        });

        var pedidosProcessados = new ConcurrentBag<Pedido>();

        try
        {
            await Parallel.ForEachAsync(pedidos,
                new ParallelOptions
                {
                    CancellationToken = token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                async (pedido, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    await ProcessarPedido(pedido, ct);
                    pedidosProcessados.Add(pedido);

                    Console.WriteLine($"Pedido #{pedido.Id} - {pedido.Cliente.Nome} - " +
                                    $"Valor: R${pedido.TotalPedido():N2} - " +
                                    $"Status: {pedido.Status}");
                });

            await monitorCancelamento;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nProcessamento cancelado pelo usuário!");
        }
        finally
        {
            cts.Dispose();
        }

        // Relatório final
        var pedidosProcessadosList = pedidosProcessados.ToList();
        var pedidosNaoProcessados = pedidos.Except(pedidosProcessadosList).ToList();

        Console.WriteLine("\nRelatório Final:");
        Console.WriteLine($"Pedidos processados: {pedidosProcessadosList.Count}");
        Console.WriteLine($"Pedidos pendentes: {pedidosNaoProcessados.Count}");
        Console.WriteLine($"Valor total processado: R${pedidosProcessadosList.Sum(p => p.TotalPedido()):N2}");

        if (pedidosNaoProcessados.Any())
        {
            Console.WriteLine("\nPedidos não processados:");
            foreach (var pedido in pedidosNaoProcessados.Take(5))
            {
                Console.WriteLine($"- #{pedido.Id} {pedido.Cliente.Nome}");
            }
            if (pedidosNaoProcessados.Count > 5)
                Console.WriteLine($"- e mais {pedidosNaoProcessados.Count - 5} pedidos...");
        }

        Console.ReadKey();
    }

    private static async Task ProcessarPedido(Pedido pedido, CancellationToken ct)
    {
        // Simula tempo de processamento variável
        await Task.Delay(new Random().Next(300, 1500), ct);

        // Verifica se foi cancelado durante o delay
        ct.ThrowIfCancellationRequested();

        // Atualiza status do pedido
        pedido.Status = StatusPedido.Processado;

        // Atualiza estoque
        foreach (var item in pedido.Itens)
        {
            item.Produto.Estoque -= item.Quantidade;
        }
    }
}

// Classes do modelo
internal enum StatusPedido
{ Pendente, Processado, Cancelado, Enviado }

internal class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; }
    public int Estoque { get; set; }
}

internal class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public Endereco Endereco { get; set; }
}

internal class Endereco
{
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string CEP { get; set; }
}

internal class Pedido
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public DateTime Data { get; set; }
    public List<ItemPedido> Itens { get; set; }
    public StatusPedido Status { get; set; }

    public decimal TotalPedido() => Itens?.Sum(i => i.PrecoUnitario * i.Quantidade) ?? 0;
}

internal class ItemPedido
{
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}