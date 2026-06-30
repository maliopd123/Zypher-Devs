namespace ProjetoChaves.Models
{
    public class Chave
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public string? Local { get; set; }
        public string? Categoria { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Disponivel { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public Usuario? UsuarioResponsavel { get; set; }
    }
}
