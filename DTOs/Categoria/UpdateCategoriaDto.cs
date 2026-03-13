namespace ControleFinanceiro.DTOs.Categoria;

using ControleFinanceiro.Entities;

public class UpdateCategoriaDto
{
    public string Descricao { get; set; }
    public FinalidadeCategoria Finalidade { get; set; }
}
