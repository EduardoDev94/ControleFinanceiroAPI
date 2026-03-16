namespace ControleFinanceiro.Services.Pessoa;

using ControleFinanceiro.DTOs.Pessoa;
using ControleFinanceiro.Entities;
using ControleFinanceiro.Repositories.Pessoa;
using ControleFinanceiro.Repositories.Transacao;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public PessoaService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
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
        var existente = await _pessoaRepository.ObterPorNomeAsync(createPessoaDto.Nome);
        if (existente != null)
            throw new ArgumentException("Já existe uma pessoa cadastrada com este nome");

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

        var existente = await _pessoaRepository.ObterPorNomeAsync(updatePessoaDto.Nome);
        if (existente != null && existente.Id != id)
            throw new ArgumentException("Já existe outra pessoa cadastrada com este nome");

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

    public async Task<IEnumerable<PessoaTotalDto>> ListarComTotaisAsync()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        var pessoaTotais = new List<PessoaTotalDto>();

        foreach (var pessoa in pessoas)
        {
            var transacoes = await _transacaoRepository.ListarPorPessoaIdAsync(pessoa.Id);

            var totalReceitas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            var saldo = totalReceitas - totalDespesas;

            pessoaTotais.Add(new PessoaTotalDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = saldo
            });
        }

        return pessoaTotais;
    }

    public async Task<TotalGeralDto> ObterTotalGeralAsync()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        decimal totalReceitasGeral = 0;
        decimal totalDespesasGeral = 0;

        foreach (var pessoa in pessoas)
        {
            var transacoes = await _transacaoRepository.ListarPorPessoaIdAsync(pessoa.Id);

            var totalReceitas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            totalReceitasGeral += totalReceitas;
            totalDespesasGeral += totalDespesas;
        }

        var saldoGeral = totalReceitasGeral - totalDespesasGeral;

        return new TotalGeralDto
        {
            TotalReceitasGeral = totalReceitasGeral,
            TotalDespesasGeral = totalDespesasGeral,
            SaldoGeral = saldoGeral
        };
    }
}
