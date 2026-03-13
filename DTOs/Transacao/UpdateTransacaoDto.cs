namespace ControleFinanceiro.DTOs.Transacao;

using ControleFinanceiro.Entities;

public class UpdateTransacaoDto
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Guid CategoriaId { get; set; }
    public Guid PessoaId { get; set; }
}
