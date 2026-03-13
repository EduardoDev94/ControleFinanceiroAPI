namespace ControleFinanceiro.Controllers;

using Microsoft.AspNetCore.Mvc;
using ControleFinanceiro.Services.Pessoa;
using ControleFinanceiro.DTOs.Pessoa;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaDto>>> ListarAsync()
    {
        var pessoas = await _pessoaService.ListarAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaDto>> ObterPorIdAsync(Guid id)
    {
        var pessoa = await _pessoaService.ObterPorIdAsync(id);
        if (pessoa == null)
            return NotFound(new { mensagem = "Pessoa não encontrada" });

        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaDto>> CriarAsync([FromBody] CreatePessoaDto createPessoaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pessoa = await _pessoaService.CriarAsync(createPessoaDto);
        return CreatedAtAction(nameof(ObterPorIdAsync), new { id = pessoa.Id }, pessoa);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PessoaDto>> AtualizarAsync(Guid id, [FromBody] UpdatePessoaDto updatePessoaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pessoa = await _pessoaService.AtualizarAsync(id, updatePessoaDto);
        if (pessoa == null)
            return NotFound(new { mensagem = "Pessoa não encontrada" });

        return Ok(pessoa);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarAsync(Guid id)
    {
        var resultado = await _pessoaService.DeletarAsync(id);
        if (!resultado)
            return NotFound(new { mensagem = "Pessoa não encontrada" });

        return NoContent();
    }
}
