using Microsoft.AspNetCore.Mvc;
using SistemaNotasFiscais.Models;
using SistemaNotasFiscais.Services;

namespace SistemaNotasFiscais.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _service;

        public NotaFiscalController(INotaFiscalService service)
        {
            _service = service;
        }

        /// <summary>
        /// Importa um arquivo XML de nota fiscal
        /// </summary>
        [HttpPost("importar-xml")]
        public async Task<IActionResult> ImportarXml([FromForm] ImportarXmlRequest request)
        {
            try
            {
                if (request.Arquivo == null || request.Arquivo.Length == 0)
                {
                    return BadRequest(new { erro = "Arquivo não fornecido" });
                }

                // Lê o conteúdo do arquivo
                string xmlConteudo;
                using (var stream = new StreamReader(request.Arquivo.OpenReadStream()))
                {
                    xmlConteudo = await stream.ReadToEndAsync();
                }

                // Processa a nota fiscal
                var nota = _service.ProcessarNotaFiscal(xmlConteudo);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Nota fiscal processada com sucesso",
                    nota = nota
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Processa XML enviado como string
        /// </summary>
        [HttpPost("processar")]
        public IActionResult ProcessarXml([FromBody] CriarNotaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.XmlConteudo))
                {
                    return BadRequest(new { erro = "XML não fornecido" });
                }

                var nota = _service.ProcessarNotaFiscal(request.XmlConteudo);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Nota fiscal processada com sucesso",
                    nota = nota
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todas as notas fiscais
        /// </summary>
        [HttpGet]
        public IActionResult ObterTodas()
        {
            try
            {
                var notas = _service.ObterTodas();
                return Ok(notas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém uma nota fiscal por ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                var nota = _service.ObterPorId(id);

                if (nota == null)
                {
                    return NotFound(new { erro = "Nota fiscal não encontrada" });
                }

                return Ok(nota);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém uma nota fiscal por número
        /// </summary>
        [HttpGet("numero/{numero}")]
        public IActionResult ObterPorNumero(string numero)
        {
            try
            {
                var nota = _service.ObterPorNumero(numero);

                if (nota == null)
                {
                    return NotFound(new { erro = "Nota fiscal não encontrada" });
                }

                return Ok(nota);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Deleta uma nota fiscal
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                var sucesso = _service.DeletarNota(id);

                if (!sucesso)
                {
                    return NotFound(new { erro = "Nota fiscal não encontrada" });
                }

                return Ok(new { sucesso = true, mensagem = "Nota fiscal deletada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}