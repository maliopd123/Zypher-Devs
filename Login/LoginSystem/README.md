# Arthur R. Couto
# LoginSystem — API de autenticação em ASP.NET Core + MySQL

Sistema de login/registro pronto para uso, com:
- ASP.NET Core 8 Web API
- Entity Framework Core + MySQL (via Pomelo)
- Senhas com hash (BCrypt) — nunca armazenadas em texto puro
- Autenticação via **JWT**
- Swagger para testar os endpoints

## 1. Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL Server rodando (local ou remoto)
- Ferramenta EF Core (uma vez só):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## 2. Configurar a conexão

Edite `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=login_system;User=root;Password=SUA_SENHA_AQUI;"
}
```

Troque também a chave JWT (`Jwt:Key`) por uma string longa e aleatória (mínimo 32 caracteres) — isso é o que assina os tokens.

> **Nunca** deixe senha/chave reais commitadas em repositório público. Em produção, use variáveis de ambiente ou um cofre de segredos (Azure Key Vault, AWS Secrets Manager, etc.), e não o appsettings.json.

## 3. Restaurar pacotes e criar o banco

```bash
cd LoginSystem
dotnet restore

# cria a primeira migration (gera a tabela Users)
dotnet ef migrations add InitialCreate

# aplica no banco MySQL (cria o banco/tabela se não existir)
dotnet ef database update
```

Se preferir, o projeto já roda `db.Database.Migrate()` automaticamente ao iniciar, então rodar `dotnet ef database update` manualmente é opcional depois da primeira vez — mas rodar a primeira `migrations add` é obrigatório.

## 4. Rodar o projeto

```bash
dotnet run
```

Acesse o Swagger em `https://localhost:{porta}/swagger` (a porta aparece no terminal) para testar os endpoints direto pelo navegador.

## 5. Endpoints

| Método | Rota                  | Descrição                              | Autenticação |
|--------|-----------------------|-----------------------------------------|---------------|
| POST   | `/api/auth/registrar` | Cria um novo usuário                    | Não           |
| POST   | `/api/auth/login`     | Autentica e retorna um token JWT        | Não           |
| GET    | `/api/auth/perfil`    | Exemplo de rota protegida               | Sim (Bearer)  |

### Exemplo — Registrar

```json
POST /api/auth/registrar
{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "senha": "minhasenha123"
}
```

### Exemplo — Login

```json
POST /api/auth/login
{
  "email": "maria@email.com",
  "senha": "minhasenha123"
}
```

Resposta:
```json
{
  "token": "eyJhbGciOi...",
  "nome": "Maria Silva",
  "email": "maria@email.com"
}
```

### Exemplo — Rota protegida

No Swagger, clique em "Authorize" e digite `Bearer {token}`, ou envie o header manualmente:

```
Authorization: Bearer eyJhbGciOi...
```

## 6. Estrutura do projeto

```
LoginSystem/
├── Controllers/
│   └── AuthController.cs      # Endpoints de registro/login/perfil
├── Data/
│   └── AppDbContext.cs        # Contexto do EF Core
├── DTOs/
│   └── AuthDtos.cs            # Objetos de entrada/saída da API
├── Models/
│   └── User.cs                # Entidade User (mapeada para o MySQL)
├── Services/
│   └── TokenService.cs        # Geração do token JWT
├── Program.cs                 # Configuração geral (DI, JWT, Swagger, CORS)
└── appsettings.json           # Connection string e config do JWT
```

## 7. Segurança — pontos já cobertos

- Senhas nunca são salvas em texto puro (hash com BCrypt, com salt automático).
- Mensagem de erro genérica no login (não revela se o e-mail existe ou não).
- E-mail único garantido por índice no banco.
- Tokens JWT assinados e com expiração configurável.

## 8. Próximos passos sugeridos (opcional)

- Adicionar refresh token para renovar o JWT sem precisar logar de novo.
- Adicionar confirmação de e-mail.
- Adicionar rate limiting no endpoint de login para evitar força bruta.
- Trocar CORS "PermitirTudo" por uma lista de origens específicas em produção.
