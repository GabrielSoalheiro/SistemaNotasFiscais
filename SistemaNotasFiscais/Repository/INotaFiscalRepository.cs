using SistemaNotasFiscais.Models;

namespace SistemaNotasFiscais.Repository
{
    public interface INotaFiscalRepository
    {
        Task<int> AdicionarAsync(NotaFiscal nota);
        Task<NotaFiscal?> BuscarPorIdAsync(int id);
        Task<List<NotaFiscal>> ListarTodosAsync();
        Task<bool> ExisteNotaComNumeroAsync(string numero);
    }
}
