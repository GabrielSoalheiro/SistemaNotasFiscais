using SistemaNotasFiscais.Models;

namespace SistemaNotasFiscais.Services
{
    public interface INotaFiscalService
    {
        NotaFiscal ProcessarNotaFiscal(string xmlConteudo);
        NotaFiscal ObterPorId(int id);
        NotaFiscal ObterPorNumero(string numero);
        IEnumerable<NotaFiscal> ObterTodas();
        bool DeletarNota(int id);
    }
}
