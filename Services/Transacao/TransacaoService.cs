namespace ControleFinanceiro.Services.Transacao;

using ControleFinanceiro.DTOs.Transacao;
using ControleFinanceiro.Entities;
using ControleFinanceiro.Repositories.Transacao;
using ControleFinanceiro.Repositories.Categoria;
using ControleFinanceiro.Repositories.Pessoa;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IPessoaRepository _pessoaRepository;

    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        ICategoriaRepository categoriaRepository,
        IPessoaRepository pessoaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _categoriaRepository = categoriaRepository;
        _pessoaRepository = pessoaRepository;
    }

    public async Task<IEnumerable<TransacaoDto>> ListarAsync()
    {
        var transacoes = await _transacaoRepository.ListarAsync();
        return transacoes.Select(t => new TransacaoDto
        {
            Id = t.Id,
            Descricao = t.Descricao,
            Valor = t.Valor,
            Tipo = t.Tipo,
            CategoriaId = t.CategoriaId,
            CategoriaNome = t.Categoria?.Descricao,
            PessoaId = t.PessoaId,
            PessoaNome = t.Pessoa?.Nome
        });
    }

    public async Task<TransacaoDto> ObterPorIdAsync(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        if (transacao == null)
            return null;

        return new TransacaoDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            CategoriaId = transacao.CategoriaId,
            CategoriaNome = transacao.Categoria?.Descricao,
            PessoaId = transacao.PessoaId,
            PessoaNome = transacao.Pessoa?.Nome
        };
    }

    public async Task<TransacaoDto> CriarAsync(CreateTransacaoDto createTransacaoDto)
    {
        // Validar se a Categoria existe
        var categoria = await _categoriaRepository.ObterPorIdAsync(createTransacaoDto.CategoriaId);
        if (categoria == null)
            throw new ArgumentException("Categoria não encontrada");

        // Validar compatibilidade entre Tipo e Finalidade da Categoria
        if (createTransacaoDto.Tipo == TipoTransacao.Despesa && 
            categoria.Finalidade == FinalidadeCategoria.Receita)
            throw new ArgumentException("A categoria não é compatível com transações do tipo Despesa");

        if (createTransacaoDto.Tipo == TipoTransacao.Receita && 
            categoria.Finalidade == FinalidadeCategoria.Despesa)
            throw new ArgumentException("A categoria não é compatível com transações do tipo Receita");

        // Validar se a Pessoa existe
        var pessoa = await _pessoaRepository.ObterPorIdAsync(createTransacaoDto.PessoaId);
        if (pessoa == null)
            throw new ArgumentException("Pessoa não encontrada");

        var transacao = new Transacao(
            createTransacaoDto.Descricao,
            createTransacaoDto.Valor,
            createTransacaoDto.Tipo,
            createTransacaoDto.CategoriaId,
            createTransacaoDto.PessoaId);

        var transacaoCriada = await _transacaoRepository.CriarAsync(transacao);

        return new TransacaoDto
        {
            Id = transacaoCriada.Id,
            Descricao = transacaoCriada.Descricao,
            Valor = transacaoCriada.Valor,
            Tipo = transacaoCriada.Tipo,
            CategoriaId = transacaoCriada.CategoriaId,
            CategoriaNome = transacaoCriada.Categoria?.Descricao,
            PessoaId = transacaoCriada.PessoaId,
            PessoaNome = transacaoCriada.Pessoa?.Nome
        };
    }

    public async Task<TransacaoDto> AtualizarAsync(Guid id, UpdateTransacaoDto updateTransacaoDto)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        if (transacao == null)
            return null;

        // Validar se a nova Categoria existe
        var categoria = await _categoriaRepository.ObterPorIdAsync(updateTransacaoDto.CategoriaId);
        if (categoria == null)
            throw new ArgumentException("Categoria não encontrada");

        // Validar compatibilidade entre Tipo e Finalidade da Categoria
        if (updateTransacaoDto.Tipo == TipoTransacao.Despesa && 
            categoria.Finalidade == FinalidadeCategoria.Receita)
            throw new ArgumentException("A categoria não é compatível com transações do tipo Despesa");

        if (updateTransacaoDto.Tipo == TipoTransacao.Receita && 
            categoria.Finalidade == FinalidadeCategoria.Despesa)
            throw new ArgumentException("A categoria não é compatível com transações do tipo Receita");

        // Validar se a Pessoa existe
        var pessoa = await _pessoaRepository.ObterPorIdAsync(updateTransacaoDto.PessoaId);
        if (pessoa == null)
            throw new ArgumentException("Pessoa não encontrada");

        transacao.Descricao = updateTransacaoDto.Descricao;
        transacao.Valor = updateTransacaoDto.Valor;
        transacao.Tipo = updateTransacaoDto.Tipo;
        transacao.CategoriaId = updateTransacaoDto.CategoriaId;
        transacao.PessoaId = updateTransacaoDto.PessoaId;

        var transacaoAtualizada = await _transacaoRepository.AtualizarAsync(transacao);

        return new TransacaoDto
        {
            Id = transacaoAtualizada.Id,
            Descricao = transacaoAtualizada.Descricao,
            Valor = transacaoAtualizada.Valor,
            Tipo = transacaoAtualizada.Tipo,
            CategoriaId = transacaoAtualizada.CategoriaId,
            CategoriaNome = transacaoAtualizada.Categoria?.Descricao,
            PessoaId = transacaoAtualizada.PessoaId,
            PessoaNome = transacaoAtualizada.Pessoa?.Nome
        };
    }

    public async Task<bool> DeletarAsync(Guid id)
    {
        return await _transacaoRepository.DeletarAsync(id);
    }
}
