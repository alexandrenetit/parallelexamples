using Bogus;

Console.WriteLine("Sistema de Consulta de Produtos - E-commerce\n");
Console.WriteLine("Iniciando busca paralela de produtos...\n");

// Configuração do Faker para produtos realistas
var produtoFaker = new Faker<Produto>("pt_BR")
    .RuleFor(p => p.Id, f => f.IndexFaker + 1)
    .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
    .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
    .RuleFor(p => p.Preco, f => f.Random.Decimal(10, 1000))
    .RuleFor(p => p.Categoria, f => f.Commerce.Categories(1)[0])
    .RuleFor(p => p.Marca, f => f.Company.CompanyName())
    .RuleFor(p => p.Estoque, f => f.Random.Int(0, 100))
    .RuleFor(p => p.DataCadastro, f => f.Date.Past(2));

// Gerar lista de IDs de produtos fictícios (50 produtos)
var produtosIds = Enumerable.Range(1, 50).ToList();

// Tempo de execução
var startTime = DateTime.Now;

await Parallel.ForEachAsync(produtosIds, async (id, ct) =>
{
    try
    {
        var produto = await BuscarProdutoApi(id, produtoFaker);

        Console.WriteLine($"[ID: {produto.Id}] {produto.Nome,-40} | " +
                        $"R${produto.Preco,8:N2} | " +
                        $"Estoque: {produto.Estoque,3} | " +
                        $"Marca: {produto.Marca,-20} | " +
                        $"Categoria: {produto.Categoria}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao buscar produto ID {id}: {ex.Message}");
    }
});

var endTime = DateTime.Now;
Console.WriteLine($"\nProcesso concluído em {(endTime - startTime).TotalSeconds:N2} segundos");

Console.ReadKey();

static async Task<Produto> BuscarProdutoApi(int id, Faker<Produto> produtoFaker)
{
    // Simula tempo de resposta variável da API (entre 500ms e 3s)
    var delay = new Random().Next(500, 3000);
    await Task.Delay(delay);

    // Simula 10% de chance de falha na API
    if (new Random().Next(1, 10) == 1)
    {
        throw new Exception("Falha na conexão com a API de produtos");
    }

    // Gera um produto fake com o ID especificado
    var produto = produtoFaker.Generate();
    produto.Id = id; // Garante que o ID seja o solicitado

    return produto;
}


class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; }
    public string Marca { get; set; }
    public int Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
}