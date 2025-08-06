using Microsoft.Data.SqlClient;
using SistemaNotasFiscais.Models;

namespace SistemaNotasFiscais.Data
{
    public class NotaFiscalRepository : RepositoryBase<Models.NotaFiscal>, INotaFiscalRepository
    {
        protected override string NomeTabela => "NotasFiscais";
        protected override string CampoId => "Id";

        public NotaFiscalRepository(DatabaseHelper db) : base(db)
        {
        }

        public override int Inserir(NotaFiscal nota)
        {
            string sql = @"
                INSERT INTO NotasFiscais 
                (Numero, CnpjPrestador, CnpjTomador, DataEmissao, DescricaoServico, ValorTotal, DataCadastro)
                VALUES 
                (@Numero, @CnpjPrestador, @CnpjTomador, @DataEmissao, @DescricaoServico, @ValorTotal, @DataCadastro);
                SELECT SCOPE_IDENTITY();";

            var parametros = new[]
            {
                new SqlParameter("@Numero", nota.Numero),
                new SqlParameter("@CnpjPrestador", nota.CnpjPrestador ?? (object)DBNull.Value),
                new SqlParameter("@CnpjTomador", nota.CnpjTomador ?? (object)DBNull.Value),
                new SqlParameter("@DataEmissao", nota.DataEmissao),
                new SqlParameter("@DescricaoServico", nota.DescricaoServico ?? (object)DBNull.Value),
                new SqlParameter("@ValorTotal", nota.ValorTotal),
                new SqlParameter("@DataCadastro", DateTime.Now)
            };

            var resultado = _db.ExecutarScalar(sql, parametros);
            return Convert.ToInt32(resultado);
        }

        public override int Atualizar(NotaFiscal nota)
        {
            string sql = @"
                UPDATE NotasFiscais SET 
                    CnpjPrestador = @CnpjPrestador,
                    CnpjTomador = @CnpjTomador,
                    DataEmissao = @DataEmissao,
                    DescricaoServico = @DescricaoServico,
                    ValorTotal = @ValorTotal,
                    DataAtualizacao = @DataAtualizacao
                WHERE Numero = @Numero";

            var parametros = new[]
            {
                new SqlParameter("@Numero", nota.Numero),
                new SqlParameter("@CnpjPrestador", nota.CnpjPrestador ?? (object)DBNull.Value),
                new SqlParameter("@CnpjTomador", nota.CnpjTomador ?? (object)DBNull.Value),
                new SqlParameter("@DataEmissao", nota.DataEmissao),
                new SqlParameter("@DescricaoServico", nota.DescricaoServico ?? (object)DBNull.Value),
                new SqlParameter("@ValorTotal", nota.ValorTotal),
                new SqlParameter("@DataAtualizacao", DateTime.Now)
            };

            return _db.ExecutarComando(sql, parametros);
        }

        public NotaFiscal ObterPorNumero(string numero)
        {
            string sql = "SELECT * FROM NotasFiscais WHERE Numero = @Numero";
            var parametros = new[] { new SqlParameter("@Numero", numero) };

            var dt = _db.ExecutarConsulta(sql, parametros);

            if (dt.Rows.Count > 0)
            {
                return MapearLinhaParaObjeto(dt.Rows[0]);
            }

            return null;
        }

        public bool ExisteNota(string numero)
        {
            string sql = "SELECT COUNT(1) FROM NotasFiscais WHERE Numero = @Numero";
            var parametros = new[] { new SqlParameter("@Numero", numero) };

            var resultado = _db.ExecutarScalar(sql, parametros);
            return Convert.ToInt32(resultado) > 0;
        }
    }
}
