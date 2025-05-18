// Cenário: Consultar estoque em API com limite de 3 chamadas simultâneas.

// Configura o gerador de produtos fake
using Bogus;

DateTime dateTime = DateTime.Now; // Marca o tempo de início

Console.WriteLine($"Data e hora de início: {dateTime}");

Console.WriteLine("Iniciando processamento paralelo de produtos...");

var produtoFaker = new Faker<Produto>("pt_BR")
    .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
    .RuleFor(p => p.Categoria, f => f.Commerce.Categories(1)[0])
    .RuleFor(p => p.Preco, f => f.Random.Decimal(10, 5000))
    .RuleFor(p => p.Marca, f => f.Company.CompanyName());

// Gera 500 produtos fake
var produtos = produtoFaker.Generate(500);

// Consulta o estoque em paralelo
var options = new ParallelOptions { MaxDegreeOfParallelism = 3 }; // Limita a 3 chamadas simultâneas

Parallel.ForEach(produtos, options, produto =>
{
    produto.Estoque = ConsultarEstoque(produto.Nome);
    Console.WriteLine($"{produto.Nome,-30} | Categoria: {produto.Categoria,-15} | Marca: {produto.Marca,-30} | Estoque: {produto.Estoque,3} unidades | Preço: R${produto.Preco,8:F2}");
});

Console.WriteLine("\nConsulta de estoque concluída para todos os 500 produtos!");

// Marca o tempo de término

Console.WriteLine($"Data e hora de término: {dateTime}");

Console.WriteLine($"Tempo total de processamento em segundos: {(dateTime - DateTime.Now).TotalMilliseconds:F2} segundos");

Console.ReadKey();


static int ConsultarEstoque(string produto)
{
    Task.Delay(1000).Wait(); // Simula API lenta
    return new Random().Next(0, 100); // Retorna valor fake
}

class Produto
{
    public string Nome { get; set; }
    public string Categoria { get; set; }
    public string Marca { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}