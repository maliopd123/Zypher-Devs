namespace ProjetoChaves.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Matricula { get; set; }

        public string Senha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }
    }
}
