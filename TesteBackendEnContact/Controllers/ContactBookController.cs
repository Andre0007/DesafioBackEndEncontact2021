using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.ContactBook;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactBookController : ControllerBase
    {
        private readonly ILogger<ContactBookController> _logger;
        public ContactBookController(ILogger<ContactBookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Insere um novo registro de livro na base do SQLite
        /// </summary>
        /// <param name="contactBook"></param>
        /// <param name="contactBookRepository"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(ContactBook contactBook, [FromServices] IContactBookRepository contactBookRepository)
        {
            _logger.LogInformation("Executando o método registrar na agenda", contactBook, contactBookRepository);
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "Registrar dados na agenda");

                contactBook.Id = 0; //esse método sempre deve ser 0 no ID, temos um HttpPut para atualizar

                return Ok(await contactBookRepository.SaveAsync(contactBook));
            }
            catch(Exception ex)
            {
                CP = 20;
                _logger.LogTrace(CP, "Erro ao salvar contato na agenda", contactBook, contactBookRepository);
                _logger.LogError(CP, ex, ex.Message, contactBook, contactBookRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um registro da agenda
        /// </summary>
        /// <param name="contactBook"></param>
        /// <param name="contactBookRepository"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(ContactBook contactBook, [FromServices] IContactBookRepository contactBookRepository)
        {
            _logger.LogInformation("Executando o método para atualizar um registro na agenda", contactBook, contactBookRepository);
            var CP = 0;

            try
            {
                return Ok(await contactBookRepository.SaveAsync(contactBook));
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogTrace(CP, "Erro ao alterar um contato na agenda", contactBook, contactBookRepository);
                _logger.LogError(CP, ex, ex.Message, contactBook, contactBookRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um registro existente na base de dados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactBookRepository"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, [FromServices] IContactBookRepository contactBookRepository)
        {
            var CP = 0;
            var Mensagem = "";

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "Deletando registro da agenda");
                var result = await contactBookRepository.DeleteAsync(id);

                Mensagem = result > 0 ? "Registro deletado com sucesso" : "Nenhum registro foi excluido";

                return Ok(Mensagem);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, id, contactBookRepository);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IContactBookRepository contactBookRepository)
        {
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "listando todos contatos");

                var response = await contactBookRepository.GetAllAsync();
                if (!response.Any())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, contactBookRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Volta um registro especifico da agenda, filtrado pelo ID do contato
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactBookRepository"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromServices] IContactBookRepository contactBookRepository)
        {
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, $"obter contato: {id}");

                var response = await contactBookRepository.GetAsync(id);
                if (response == null)
                    return NotFound("Nenhum contato encontrado");

                return Ok(response);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, contactBookRepository);

                return BadRequest(ex.Message);
            }
        }
    }
}
