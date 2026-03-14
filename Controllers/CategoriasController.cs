namespace ControleFinanceiro.Controllers;

using Microsoft.AspNetCore.Mvc;
using ControleFinanceiro.Services.Categoria;
using ControleFinanceiro.DTOs.Categoria;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> ListarAsync()
    {
        var categorias = await _categoriaService.ListarAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> ObterPorIdAsync(Guid id)
    {
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        if (categoria == null)
            return NotFound(new { mensagem = "Categoria não encontrada" });

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> CriarAsync([FromBody] CreateCategoriaDto createCategoriaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoria = await _categoriaService.CriarAsync(createCategoriaDto);
        return CreatedAtAction("ObterPorId", new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoriaDto>> AtualizarAsync(Guid id, [FromBody] UpdateCategoriaDto updateCategoriaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoria = await _categoriaService.AtualizarAsync(id, updateCategoriaDto);
        if (categoria == null)
            return NotFound(new { mensagem = "Categoria não encontrada" });

        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarAsync(Guid id)
    {
        var resultado = await _categoriaService.DeletarAsync(id);
        if (!resultado)
            return NotFound(new { mensagem = "Categoria não encontrada" });

        return NoContent();
    }
}
