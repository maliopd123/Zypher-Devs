namespace ProjetoChaves.Models
{
    public class Negacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int ChaveId { get; set; }
        public Chave? Chave { get; set; }
        public DateTime DataHora { get; set; }
        public string? Motivo { get; set; }
        public string? Local { get; set; }
    }
}
