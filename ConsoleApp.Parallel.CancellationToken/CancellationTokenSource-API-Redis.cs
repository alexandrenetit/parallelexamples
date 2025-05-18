using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

//[ApiController]
//[Route("processamentos")]
//public class ProcessamentoController : ControllerBase
//{
//    private readonly IConnectionMultiplexer _redis;
//    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _processos;

//    [HttpPost("iniciar")]
//    public async Task<IActionResult> Iniciar()
//    {
//        var processId = Guid.NewGuid();
//        var cts = new CancellationTokenSource();

//        _processos.TryAdd(processId, cts);
//        await _redis.GetDatabase().StringSetAsync($"process:{processId}", "ativo");

//        _ = Task.Run(async () => await ProcessarPedidos(cts.Token));

//        return Ok(new { ProcessId = processId });
//    }

//    [HttpPost("cancelar/{processId}")]
//    public IActionResult Cancelar(Guid processId)
//    {
//        if (_processos.TryRemove(processId, out var cts))
//        {
//            cts.Cancel();
//            cts.Dispose();
//            return Ok();
//        }
//        return NotFound();
//    }
//}


/* Outro cenário, mas registrando no warmup da aplicação  */

//// Startup.cs
//services.AddScoped<CancellationTokenSource>();

//// Controller
//public class PedidoController : ControllerBase
//{
//    private readonly CancellationTokenSource _cts;

//    public PedidoController(CancellationTokenSource cts)
//    {
//        _cts = cts;
//    }

//    [HttpPost("cancelar")]
//    public IActionResult Cancelar()
//    {
//        _cts.Cancel();
//        return Ok();
//    }
//}