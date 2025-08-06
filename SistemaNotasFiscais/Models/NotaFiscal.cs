namespace SistemaNotasFiscais.Models
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string CnpjPrestador { get; set; }
        public string CnpjTomador { get; set; }
        public DateTime DataEmissao { get; set; }
        public string DescricaoServico { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CriarNotaRequest
    {
        public string XmlConteudo { get; set; }
    }

    public class ImportarXmlRequest
    {
        public IFormFile Arquivo { get; set; }
    }
}
