# UneCont - API de Notas Fiscais

## 📋 Estrutura do Projeto

```
UneCont.NotaFiscal.Api/
├── Controllers/
│   └── NotaFiscalController.cs
├── Models/
│   └── NotaFiscal.cs (inclui CriarNotaRequest e ImportarXmlRequest)
├── Services/
│   ├── INotaFiscalService.cs
│   └── NotaFiscalService.cs
├── Data/
│   ├── DatabaseHelper.cs
│   ├── RepositoryBase.cs (genérico, reutilizável)
│   ├── INotaFiscalRepository.cs
│   └── NotaFiscalRepository.cs
├── Xml/
│   └── XmlProcessor.cs
├── Program.cs
└── appsettings.json
```

## 🚀 Como Executar

### 1. Configurar o Banco de Dados

Edite o `appsettings.json` e ajuste a connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=UneCont;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 2. Executar a Aplicação

```bash
dotnet run
```

A aplicação criará automaticamente a tabela NotasFiscais no primeiro uso.

### 3. Acessar o Swagger

Abra o navegador em: `https://localhost:[porta]/`

## 📝 Endpoints Disponíveis

### 1. Importar XML via Upload de Arquivo
- **POST** `/api/NotaFiscal/importar-xml`
- **Form-data:** arquivo XML
- Processa o arquivo XML e salva/atualiza no banco

### 2. Processar XML via String
- **POST** `/api/NotaFiscal/processar`
- **Body JSON:**
```json
{
  "xmlConteudo": "..."
}
```

### 3. Listar Todas as Notas
- **GET** `/api/NotaFiscal`
- Retorna todas as notas cadastradas

### 4. Buscar por ID
- **GET** `/api/NotaFiscal/{id}`
- Retorna uma nota específica

### 5. Buscar por Número
- **GET** `/api/NotaFiscal/numero/{numero}`
- Retorna a nota pelo número

### 6. Deletar Nota
- **DELETE** `/api/NotaFiscal/{id}`
- Remove uma nota do banco

## 🎯 Características da Solução

### ✅ Pontos Fortes

#### **Simplicidade e Clareza**
- Código em português conforme solicitado
- Nomes de funções e variáveis autodescritivos
- Estrutura limpa e organizada

#### **ADO.NET Puro**
- Sem Entity Framework
- Controle total sobre as queries SQL
- Performance otimizada

#### **Repository Genérico**
- `RepositoryBase` reutilizável para outros models
- Métodos CRUD básicos implementados
- Fácil extensão para novas entidades

#### **Tratamento de Duplicatas**
- Atualiza automaticamente notas existentes
- Mantém histórico com DataCadastro e DataAtualizacao

#### **Criação Automática do Banco**
- Cria a tabela automaticamente se não existir
- Não precisa rodar scripts SQL manualmente

#### **API RESTful Completa**
- Swagger integrado para testes
- Múltiplas formas de importação (arquivo ou string)
- Respostas padronizadas com mensagens claras

## 🔄 Fluxo de Processamento

1. **Recebimento do XML** → Controller recebe arquivo ou string
2. **Processamento** → XmlProcessor extrai os dados
3. **Verificação** → Service verifica se a nota já existe
4. **Persistência** → Repository salva ou atualiza no banco
5. **Resposta** → Controller retorna o resultado

## 📊 Estrutura do Banco de Dados

```sql
CREATE TABLE NotasFiscais (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero VARCHAR(50) NOT NULL UNIQUE,
    CnpjPrestador VARCHAR(20),
    CnpjTomador VARCHAR(20),
    DataEmissao DATETIME,
    DescricaoServico NVARCHAR(500),
    ValorTotal DECIMAL(18,2),
    DataCadastro DATETIME DEFAULT GETDATE(),
    DataAtualizacao DATETIME NULL
)
```

## 🔧 Possíveis Melhorias Futuras

### **Validações**
- Validar CNPJ
- Validar formato de data
- Validar valores negativos

### **Logs**
- Adicionar Serilog ou NLog
- Registrar todas as operações

### **Cache**
- Implementar cache para consultas frequentes

### **Paginação**
- Adicionar paginação na listagem de notas

### **Filtros**
- Buscar por período
- Buscar por CNPJ
- Buscar por valor

### **Autenticação**
- Adicionar JWT para segurança

### **Testes**
- Adicionar testes unitários
- Adicionar testes de integração

## 🎨 Para a Apresentação

### **Demonstrar:**
- Upload de arquivo XML via Swagger
- Processamento mostrando inserção inicial
- Reprocessamento do mesmo XML mostrando atualização
- Consulta de todas as notas
- Busca específica por número

### **Destacar:**
- Código limpo e em português
- Repository genérico reutilizável
- Tratamento automático de duplicatas
- Sem dependências externas (apenas ADO.NET)
- Estrutura profissional e escalável
