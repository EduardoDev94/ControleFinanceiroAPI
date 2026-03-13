namespace ControleFinanceiro.Services.Transacao;

using ControleFinanceiro.DTOs.Transacao;
using ControleFinanceiro.Entities;

public interface ITransacaoService
{
    Task<IEnumerable<TransacaoDto>> ListarAsync();
    Task<TransacaoDto> ObterPorIdAsync(Guid id);
    Task<TransacaoDto> CriarAsync(CreateTransacaoDto createTransacaoDto);
    Task<TransacaoDto> AtualizarAsync(Guid id, UpdateTransacaoDto updateTransacaoDto);
    Task<bool> DeletarAsync(Guid id);
}
