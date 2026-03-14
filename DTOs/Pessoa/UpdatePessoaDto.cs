using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.DTOs.Pessoa;

public class UpdatePessoaDto
{
    public string Nome { get; set; }

    public int Idade { get; set; }
}
