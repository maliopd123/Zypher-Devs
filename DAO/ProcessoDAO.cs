using MySql.Data.MySqlClient;

namespace ProjetoChaves.DAO;

public class ProcessoDAO
{
    private readonly string _connectionString;

    public ProcessoDAO(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConexaoMySQL")
            ?? throw new InvalidOperationException("A conexão 'ConexaoMySQL' não foi configurada.");
    }

    // A classe Processo fica dentro do próprio DAO para não precisar criar Models/Processo.cs.
    public class Processo
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
    }

    // CADASTRAR
    public void Cadastrar(Processo processo)
    {
        const string sql = """
            INSERT INTO processos
                (numero_pro, assunto_pro, descricao_pro, status_pro, data_abertura_pro)
            VALUES
                (@numero, @assunto, @descricao, @status, @dataAbertura);
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@numero", processo.Numero);
        command.Parameters.AddWithValue("@assunto", processo.Assunto);
        command.Parameters.AddWithValue("@descricao", processo.Descricao);
        command.Parameters.AddWithValue("@status", processo.Status);
        command.Parameters.AddWithValue("@dataAbertura", processo.DataAbertura);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // LISTAR
    public List<Processo> Listar()
    {
        const string sql = """
            SELECT id_pro, numero_pro, assunto_pro, descricao_pro,
                   status_pro, data_abertura_pro
            FROM processos
            ORDER BY id_pro;
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);

        connection.Open();
        using var reader = command.ExecuteReader();

        var processos = new List<Processo>();

        while (reader.Read())
        {
            processos.Add(CriarProcesso(reader));
        }

        return processos;
    }

    // BUSCAR
    public Processo? Buscar(int id)
    {
        const string sql = """
            SELECT id_pro, numero_pro, assunto_pro, descricao_pro,
                   status_pro, data_abertura_pro
            FROM processos
            WHERE id_pro = @id
            LIMIT 1;
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        using var reader = command.ExecuteReader();

        return reader.Read() ? CriarProcesso(reader) : null;
    }

    // EDITAR
    public void Editar(Processo processo)
    {
        const string sql = """
            UPDATE processos
            SET numero_pro = @numero,
                assunto_pro = @assunto,
                descricao_pro = @descricao,
                status_pro = @status,
                data_abertura_pro = @dataAbertura
            WHERE id_pro = @id;
            """;

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@numero", processo.Numero);
        command.Parameters.AddWithValue("@assunto", processo.Assunto);
        command.Parameters.AddWithValue("@descricao", processo.Descricao);
        command.Parameters.AddWithValue("@status", processo.Status);
        command.Parameters.AddWithValue("@dataAbertura", processo.DataAbertura);
        command.Parameters.AddWithValue("@id", processo.Id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // EXCLUIR
    public void Excluir(int id)
    {
        const string sql = "DELETE FROM processos WHERE id_pro = @id;";

        using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    private static Processo CriarProcesso(MySqlDataReader reader)
    {
        return new Processo
        {
            Id = reader.GetInt32("id_pro"),
            Numero = reader.GetString("numero_pro"),
            Assunto = reader.GetString("assunto_pro"),
            Descricao = reader.IsDBNull(reader.GetOrdinal("descricao_pro"))
                ? string.Empty
                : reader.GetString("descricao_pro"),
            Status = reader.GetString("status_pro"),
            DataAbertura = reader.GetDateTime("data_abertura_pro")
        };
    }
}
