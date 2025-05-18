//    Cenário: Gerar 3 relatórios simultaneamente.
//📌  Vantagens:
//✔   Executa múltiplas tarefas independentes em paralelo
//✔   Reduz tempo total de processamento

Parallel.Invoke(
            () => GerarRelatorio("Vendas por Categoria"),
            () => GerarRelatorio("Clientes Ativos"),
            () => GerarRelatorio("Produtos em Falta")
        );

Console.ReadKey();

static void GerarRelatorio(string nome)
{
    Console.WriteLine($"Iniciando {nome}...");
    Task.Delay(2000).Wait(); // Simula processamento
    Console.WriteLine($"{nome} gerado!");
}