namespace ControleFinanceiro.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pessoa
{
    public Pessoa() { }

    // Construtor para Criação
    public Pessoa(string nome, int idade)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Idade = idade;
    }

    // Construtor para Alteração/Carregamento
    public Pessoa(Guid id, string nome, int idade)
    {
        Id = id;
        Nome = nome;
        Idade = idade;
    }

    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(0, 100, ErrorMessage = "A idade deve estar entre 0 e 150 anos")]
    public int Idade { get; set; }

}



