namespace ControleFinanceiro.Services.Pessoa;

using ControleFinanceiro.DTOs.Pessoa;
using ControleFinanceiro.Entities;
using ControleFinanceiro.Repositories.Pessoa;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoaService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public async Task<IEnumerable<PessoaDto>> ListarAsync()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        return pessoas.Select(p => new PessoaDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Idade = p.Idade
        });
    }

    public async Task<PessoaDto> ObterPorIdAsync(Guid id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        if (pessoa == null)
            return null;

        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        };
    }

    public async Task<PessoaDto> CriarAsync(CreatePessoaDto createPessoaDto)
    {
        var pessoa = new Pessoa(createPessoaDto.Nome, createPessoaDto.Idade);
        var pessoaCriada = await _pessoaRepository.CriarAsync(pessoa);

        return new PessoaDto
        {
            Id = pessoaCriada.Id,
            Nome = pessoaCriada.Nome,
            Idade = pessoaCriada.Idade
        };
    }

    public async Task<PessoaDto> AtualizarAsync(Guid id, UpdatePessoaDto updatePessoaDto)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        if (pessoa == null)
            return null;

        pessoa.Nome = updatePessoaDto.Nome;
        pessoa.Idade = updatePessoaDto.Idade;

        var pessoaAtualizada = await _pessoaRepository.AtualizarAsync(pessoa);

        return new PessoaDto
        {
            Id = pessoaAtualizada.Id,
            Nome = pessoaAtualizada.Nome,
            Idade = pessoaAtualizada.Idade
        };
    }

    public async Task<bool> DeletarAsync(Guid id)
    {
        return await _pessoaRepository.DeletarAsync(id);
    }
}
