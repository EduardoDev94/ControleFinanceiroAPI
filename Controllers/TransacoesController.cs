namespace ControleFinanceiro.Controllers;

using Microsoft.AspNetCore.Mvc;
using ControleFinanceiro.Services.Transacao;
using ControleFinanceiro.DTOs.Transacao;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacaoDto>>> ListarAsync()
    {
        var transacoes = await _transacaoService.ListarAsync();
        return Ok(transacoes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransacaoDto>> ObterPorIdAsync(Guid id)
    {
        var transacao = await _transacaoService.ObterPorIdAsync(id);
        if (transacao == null)
            return NotFound(new { mensagem = "Transação não encontrada" });

        return Ok(transacao);
    }

    [HttpPost]
    public async Task<ActionResult<TransacaoDto>> CriarAsync([FromBody] CreateTransacaoDto createTransacaoDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var transacao = await _transacaoService.CriarAsync(createTransacaoDto);
            return CreatedAtAction(nameof(ObterPorIdAsync), new { id = transacao.Id }, transacao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TransacaoDto>> AtualizarAsync(Guid id, [FromBody] UpdateTransacaoDto updateTransacaoDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var transacao = await _transacaoService.AtualizarAsync(id, updateTransacaoDto);
            if (transacao == null)
                return NotFound(new { mensagem = "Transação não encontrada" });

            return Ok(transacao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarAsync(Guid id)
    {
        var resultado = await _transacaoService.DeletarAsync(id);
        if (!resultado)
            return NotFound(new { mensagem = "Transação não encontrada" });

        return NoContent();
    }
}
