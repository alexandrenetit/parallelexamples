// Cenário: Processar 1000 pedidos pendentes e atualizar para "Enviado".

var pedidos = GerarPedidosFake(1000); // Gera 1000 pedidos fake

Console.WriteLine("Iniciando processamento paralelo de pedidos...");

DateTime dateTime = DateTime.Now; // Marca o tempo de início

Console.WriteLine($"Data e hora de início: {dateTime}");    

Parallel.For(0, pedidos.Count, i => 
{
    // Simula o processamento de cada pedido
    pedidos[i].Status = "Enviado"; // Atualiza o status do pedido
    Console.WriteLine($"Pedido {pedidos[i].Id} atualizado na thread {Task.CurrentId}");
});

dateTime = DateTime.Now;

// Marca o tempo de término

Console.WriteLine($"Data e hora de término: {dateTime}");

Console.WriteLine($"Tempo total de processamento em segundos: {(dateTime - DateTime.Now).TotalMilliseconds} segundos");

Console.WriteLine($"{pedidos.Count} pedidos processados!");

Console.ReadKey();

static List<Pedido> GerarPedidosFake(int quantidade)
{
    var pedidos = new List<Pedido>();
    for (int i = 1; i <= quantidade; i++)
    {
        pedidos.Add(new Pedido { Id = i, Status = "Pendente" });
    }
    return pedidos;
}

class Pedido
{
    public int Id { get; set; }
    public string Status { get; set; }
}