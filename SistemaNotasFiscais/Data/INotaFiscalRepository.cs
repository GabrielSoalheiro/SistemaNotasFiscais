using SistemaNotasFiscais.Models;

namespace SistemaNotasFiscais.Data
{
    public interface INotaFiscalRepository : IRepositoryBase<NotaFiscal>
    {
        NotaFiscal ObterPorNumero(string numero);
        bool ExisteNota(string numero);
    }
}
