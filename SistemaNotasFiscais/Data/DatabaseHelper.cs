using Microsoft.Data.SqlClient;
using System.Data;

namespace SistemaNotasFiscais.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CriarConexao()
        {
            return new SqlConnection(_connectionString);
        }

        public void CriarTabelaSeNaoExistir()
        {
            try
            {
                // Primeiro, tenta criar o banco se não existir
                CriarBancoSeNaoExistir();

                // Depois cria a tabela
                string sql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='NotasFiscais' AND xtype='U')
                    CREATE TABLE NotasFiscais (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Numero VARCHAR(50) NOT NULL,
                        CnpjPrestador VARCHAR(20),
                        CnpjTomador VARCHAR(20),
                        DataEmissao DATETIME,
                        DescricaoServico NVARCHAR(500),
                        ValorTotal DECIMAL(18,2),
                        DataCadastro DATETIME DEFAULT GETDATE(),
                        DataAtualizacao DATETIME NULL,
                        CONSTRAINT UQ_NotaFiscal_Numero UNIQUE(Numero)
                    )";

                using (var conexao = CriarConexao())
                {
                    conexao.Open();
                    using (var comando = new SqlCommand(sql, conexao))
                    {
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar tabela: {ex.Message}", ex);
            }
        }

        private void CriarBancoSeNaoExistir()
        {
            // Conecta no master para criar o banco
            var connectionStringMaster = _connectionString.Replace("Database=UneCont", "Database=master");

            string sql = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'UneCont')
                BEGIN
                    CREATE DATABASE UneCont
                END";

            using (var conexao = new SqlConnection(connectionStringMaster))
            {
                conexao.Open();
                using (var comando = new SqlCommand(sql, conexao))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }

        public int ExecutarComando(string sql, SqlParameter[] parametros = null)
        {
            using (var conexao = CriarConexao())
            {
                conexao.Open();
                using (var comando = new SqlCommand(sql, conexao))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    return comando.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecutarConsulta(string sql, SqlParameter[] parametros = null)
        {
            using (var conexao = CriarConexao())
            {
                conexao.Open();
                using (var comando = new SqlCommand(sql, conexao))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }

                    using (var adapter = new SqlDataAdapter(comando))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public object ExecutarScalar(string sql, SqlParameter[] parametros = null)
        {
            using (var conexao = CriarConexao())
            {
                conexao.Open();
                using (var comando = new SqlCommand(sql, conexao))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    return comando.ExecuteScalar();
                }
            }
        }
    }
}