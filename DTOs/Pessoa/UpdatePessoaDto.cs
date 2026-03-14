using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.DTOs.Pessoa;

public class UpdatePessoaDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(0, 100, ErrorMessage = "A idade deve estar entre 0 e 100 anos")]
    public int Idade { get; set; }
}
