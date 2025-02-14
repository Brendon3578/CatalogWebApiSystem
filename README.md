# CatalogWebApiSystem

Este projeto é uma API RESTful desenvolvida em C# utilizando o ASP.NET Core, destinada ao gerenciamento de um catálogo de produtos e categorias. A aplicação permite operações de criação, leitura, atualização e exclusão (CRUD) de produtos e categorias, com persistência dos dados em um banco de dados SQL Server.

## 💻 Descrição

O sistema foi projetado para facilitar o gerenciamento de produtos e suas respectivas categorias, oferecendo endpoints para operações CRUD e consultas específicas.

## 🔮 Funcionalidades

- **Gerenciamento de Produtos**: Permite criar, listar, atualizar e remover produtos.
- **Gerenciamento de Categorias**: Possibilita a criação, listagem, atualização e remoção de categorias.
- **Consulta de Produtos por Categoria**: Permite listar produtos filtrados por categoria.

## 📊 Estrutura das Entidades

### Product (Produto)

- **Atributos**: `Id`, `Name`, `Price`, `Description`, `CategoryId`, `ImageUrl`, `CreatedOn`, `Stock`
- **Relacionamento**: Muitos-para-um com `Category`

### Category (Categoria)

- **Atributos**: `Id`, `Name`, `ImageUrl`
- **Relacionamento**: Um-para-muitos com `Product`

## 🛠️ Tecnologias Utilizadas

- **C# .NET Core**
- **ASP.NET Core**
- **Entity Framework Core**
- **MySQL**

## ✨ Padrões de Projetos Aplicados

- **Repository Pattern**: Utilizado para abstrair e isolar a lógica de acesso aos dados.
- **Unit of Work**: Implementado para gerenciar transações e garantir a consistência dos dados.
- **Dependency Injection**: Utilizado para registrar e resolver dependências.
- **Migrations**: Utilizado para versionar o banco de dados e aplicar alterações de esquema.
- **Fluent Validation**: Utilizado para validação de dados de entrada.
- **Scalar**: Utilizado para documentar a API via OpenAPI.
- **Princípios SOLID**: Utilizado para garantir a qualidade do código e seguir as boas práticas de programação.

## 📂 Estrutura do Projeto

- **Controllers**: Contém os controladores da API que lidam com as requisições HTTP.
- **Domain**: Inclui as classes de domínio e Entidades utilizados na aplicação.
    - **Models**: Define as entidades e modelos de dados utilizados na aplicação.
    - **Validations**: Contém as classes de validação de dados utilizando o Fluent Validation.
- **DataAccess**: Inclui o contexto do banco de dados e configurações do Entity Framework Core, támbém utilizando o padrão de projeto Unit of Work.
    - **Repositories**: Contém as implementações dos repositórios para acesso aos dados.
    - **Interfaces**: Define as interfaces dos repositórios e do Unit of Work.
- **Filters**: Contém os filtros de exceção personalizados.
- **Extensions**: Contém classes de extensão para a aplicação.
- **Logging**: Contém as classes de configuração do Logger personalizado que salva os registros em uma pasta /Log.
- **Migrations**: Contém as migrações do banco de dados geradas pelo Entity Framework Core.

## 🚀 Configuração do Ambiente

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/Brendon3578/CatalogWebApiSystem.git
   ```

2. **Navegue até o diretório do projeto:**

   ```bash
   cd CatalogWebApiSystem
   ```

3. **Restaure as dependências:**

   ```bash
   dotnet restore
   ```

4. **Configure a string de conexão para o SQL Server no `appsettings.json`.**

5. **Execute as migrações para criar as tabelas no banco de dados:**

   ```bash
   dotnet ef database update
   ```

6. **Inicie a aplicação:**

   ```bash
   dotnet run
   ```

A API estará disponível em `https://localhost:5001` ou conforme configurado.

## 🧪 Testes

Para executar os testes, utilize o seguinte comando:

```bash
dotnet test
```

---

<h3 align="center">
    Feito com ☕ por <a href="https://github.com/Brendon3578">Brendon Gomes</a>
</h3>
