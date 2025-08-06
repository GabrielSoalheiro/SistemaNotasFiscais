using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace SistemaNotasFiscais.Data
{
    public interface IRepositoryBase<T> where T : class, new()
    {
        T ObterPorId(int id);
        IEnumerable<T> ObterTodos();
        int Inserir(T entidade);
        int Atualizar(T entidade);
        int Deletar(int id);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        protected readonly DatabaseHelper _db;
        protected abstract string NomeTabela { get; }
        protected abstract string CampoId { get; }

        public RepositoryBase(DatabaseHelper db)
        {
            _db = db;
        }

        public virtual T ObterPorId(int id)
        {
            string sql = $"SELECT * FROM {NomeTabela} WHERE {CampoId} = @Id";
            var parametros = new[] { new SqlParameter("@Id", id) };

            var dt = _db.ExecutarConsulta(sql, parametros);

            if (dt.Rows.Count > 0)
            {
                return MapearLinhaParaObjeto(dt.Rows[0]);
            }

            return null;
        }

        public virtual IEnumerable<T> ObterTodos()
        {
            string sql = $"SELECT * FROM {NomeTabela}";
            var dt = _db.ExecutarConsulta(sql);

            var lista = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearLinhaParaObjeto(row));
            }

            return lista;
        }

        public abstract int Inserir(T entidade);
        public abstract int Atualizar(T entidade);

        public virtual int Deletar(int id)
        {
            string sql = $"DELETE FROM {NomeTabela} WHERE {CampoId} = @Id";
            var parametros = new[] { new SqlParameter("@Id", id) };

            return _db.ExecutarComando(sql, parametros);
        }

        protected T MapearLinhaParaObjeto(DataRow row)
        {
            T obj = new T();
            Type tipo = typeof(T);

            foreach (DataColumn coluna in row.Table.Columns)
            {
                PropertyInfo propriedade = tipo.GetProperty(coluna.ColumnName);

                if (propriedade != null && row[coluna] != DBNull.Value)
                {
                    Type tipoPropriedade = Nullable.GetUnderlyingType(propriedade.PropertyType) ?? propriedade.PropertyType;
                    object valorConvertido = Convert.ChangeType(row[coluna], tipoPropriedade);
                    propriedade.SetValue(obj, valorConvertido);
                }
            }

            return obj;
        }

        protected SqlParameter[] CriarParametros(Dictionary<string, object> valores)
        {
            var parametros = new List<SqlParameter>();

            foreach (var item in valores)
            {
                var parametro = new SqlParameter($"@{item.Key}", item.Value ?? DBNull.Value);
                parametros.Add(parametro);
            }

            return parametros.ToArray();
        }
    }
}
