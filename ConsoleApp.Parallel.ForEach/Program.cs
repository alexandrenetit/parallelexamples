// Configura o gerador de dados fake para produtos
using Bogus;

var produtoFaker = new Faker<Produto>("pt_BR")
    .RuleFor(p => p.Id, f => f.IndexFaker + 1)
    .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
    .RuleFor(p => p.PesoKg, f => f.Random.Decimal(0.1m, 20m)); // Peso entre 0.1kg e 20kg

// Gera 500 produtos fake
var produtos = produtoFaker.Generate(500);

// Calcula frete em paralelo
Parallel.ForEach(produtos, produto =>
{
    produto.Frete = CalcularFrete(produto.PesoKg);
    Console.WriteLine($"Frete para {produto.Nome,-40} (Peso: {produto.PesoKg:F2}kg): R$ {produto.Frete:F2}");
});

Console.WriteLine("\nTodos os fretes calculados!");

Console.ReadKey();

static decimal CalcularFrete(decimal peso)
{
    // Simula cálculo complexo
    Task.Delay(10).Wait(); // Espera 10ms (simula API)
    return peso * 10m; // R$10 por kg
}

internal class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal PesoKg { get; set; }
    public decimal Frete { get; set; }
}