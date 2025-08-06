using SistemaNotasFiscais.Data;
using SistemaNotasFiscais.Xml;

namespace SistemaNotasFiscais.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _repository;
        private readonly XmlProcessor _xmlProcessor;

        public NotaFiscalService(INotaFiscalRepository repository)
        {
            _repository = repository;
            _xmlProcessor = new XmlProcessor();
        }

        public Models.NotaFiscal ProcessarNotaFiscal(string xmlConteudo)
        {
            try
            {
                // Processa o XML e extrai os dados
                var nota = _xmlProcessor.ProcessarXml(xmlConteudo);

                // Verifica se a nota já existe
                var notaExistente = _repository.ObterPorNumero(nota.Numero);

                if (notaExistente != null)
                {
                    // Atualiza a nota existente
                    nota.Id = notaExistente.Id;
                    nota.DataCadastro = notaExistente.DataCadastro;
                    nota.DataAtualizacao = DateTime.Now;

                    _repository.Atualizar(nota);

                    return nota;
                }
                else
                {
                    // Insere nova nota
                    nota.DataCadastro = DateTime.Now;
                    var id = _repository.Inserir(nota);
                    nota.Id = id;

                    return nota;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar nota fiscal: {ex.Message}", ex);
            }
        }

        public Models.NotaFiscal ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public Models.NotaFiscal ObterPorNumero(string numero)
        {
            return _repository.ObterPorNumero(numero);
        }

        public IEnumerable<Models.NotaFiscal> ObterTodas()
        {
            return _repository.ObterTodos();
        }

        public bool DeletarNota(int id)
        {
            return _repository.Deletar(id) > 0;
        }
    }
}
