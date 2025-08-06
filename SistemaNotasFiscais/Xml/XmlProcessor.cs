using SistemaNotasFiscais.Models;
using System.Xml.Linq;

namespace SistemaNotasFiscais.Xml
{
    public class XmlProcessor
    {
        public NotaFiscal ProcessarXml(string xmlConteudo)
        {
            try
            {
                // Carrega o XML
                XDocument doc = XDocument.Parse(xmlConteudo);
                XElement raiz = doc.Root;

                // Extrai as informações básicas
                var nota = new NotaFiscal
                {
                    Numero = ObterValorElemento(raiz, "Numero"),
                    DataEmissao = ConverterParaData(ObterValorElemento(raiz, "DataEmissao")),
                    ValorTotal = ConverterParaDecimal(ObterValorElemento(raiz, "Valor")),
                    DataCadastro = DateTime.Now
                };

                // Extrai informações do Prestador
                XElement prestador = raiz.Element("Prestador");
                if (prestador != null)
                {
                    nota.CnpjPrestador = ObterValorElemento(prestador, "CNPJ");
                }

                // Extrai informações do Tomador  
                XElement tomador = raiz.Element("Tomador");
                if (tomador != null)
                {
                    nota.CnpjTomador = ObterValorElemento(tomador, "CNPJ");
                }

                // Extrai informações do Serviço
                XElement servico = raiz.Element("Servico");
                if (servico != null)
                {
                    nota.DescricaoServico = ObterValorElemento(servico, "Descricao");
                }

                return nota;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar XML: {ex.Message}", ex);
            }
        }

        private string ObterValorElemento(XElement elemento, string nomeFilho)
        {
            var filho = elemento?.Element(nomeFilho);
            return filho?.Value ?? string.Empty;
        }

        private DateTime ConverterParaData(string dataString)
        {
            if (DateTime.TryParse(dataString, out DateTime data))
            {
                return data;
            }
            return DateTime.MinValue;
        }

        private decimal ConverterParaDecimal(string valorString)
        {
            // Remove possíveis caracteres de formatação
            valorString = valorString?.Replace("R$", "")
                                    .Replace(".", "")
                                    .Replace(",", ".")
                                    .Trim() ?? "0";

            if (decimal.TryParse(valorString,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal valor))
            {
                return valor;
            }
            return 0;
        }
    }
}
