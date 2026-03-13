namespace ControleFinanceiro.Repositories.Pessoa;

using ControleFinanceiro.Entities;

public interface IPessoaRepository
{
    Task<IEnumerable<Pessoa>> ListarAsync();
    Task<Pessoa> ObterPorIdAsync(Guid id);
    Task<Pessoa> CriarAsync(Pessoa pessoa);
    Task<Pessoa> AtualizarAsync(Pessoa pessoa);
    Task<bool> DeletarAsync(Guid id);
}
