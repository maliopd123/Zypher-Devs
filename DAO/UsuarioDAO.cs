using MySql.Data.MySqlClient;
using ProjetoChaves.Models;

namespace ProjetoChaves.DAO;

public class UsuarioDAO
{
    private readonly string _connectionString;

    public UsuarioDAO(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConexaoMySQL")
            ?? throw new InvalidOperationException("A conexão 'ConexaoMySQL' não foi configurada.");
    }

    public void Inserir(Usuario usuario)
    {
        const string sql = """
            INSERT INTO usuarios (nome_usu, email_usu, senha_usu)
            VALUES (@nome, @email, @senha);
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@nome", usuario.Nome);
        command.Parameters.AddWithValue("@email", usuario.Email);
        command.Parameters.AddWithValue("@senha", usuario.Senha);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public List<Usuario> Listar()
    {
        const string sql = "SELECT id_usu, nome_usu, email_usu, senha_usu FROM usuarios ORDER BY nome_usu;";
        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        var usuarios = new List<Usuario>();
        while (reader.Read())
        {
            usuarios.Add(CriarUsuario(reader));
        }

        return usuarios;
    }

    public Usuario? BuscarPorEmail(string email)
    {
        const string sql = """
            SELECT id_usu, nome_usu, email_usu, senha_usu
            FROM usuarios
            WHERE email_usu = @email
            LIMIT 1;
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@email", email);
        connection.Open();
        using var reader = command.ExecuteReader();
        return reader.Read() ? CriarUsuario(reader) : null;
    }

    private static Usuario CriarUsuario(MySqlDataReader reader)
    {
        return new Usuario
        {
            Id = reader.GetInt32("id_usu"),
            Nome = reader.GetString("nome_usu"),
            Email = reader.GetString("email_usu"),
            Senha = reader.GetString("senha_usu"),
            Ativo = true
        };
    }
}
