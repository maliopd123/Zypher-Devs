using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // Armazena apenas o hash da senha, nunca a senha em texto puro
        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
