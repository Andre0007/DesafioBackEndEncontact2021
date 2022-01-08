using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Controllers.Models;
using TesteBackendEnContact.Core.Interface.ContactBook.Company;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Insere um novo registro da empresa
        /// </summary>
        /// <param name="company"></param>
        /// <param name="companyRepository"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ICompany>> Post(SaveCompanyRequest company, [FromServices] ICompanyRepository companyRepository)
        {
            _logger.LogInformation("Executando o método registrar empresa", company, companyRepository);
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "Registrar dados da empresa");

                company.Id = 0; //esse método sempre deve ser 0 no ID, temos um HttpPut para atualizar

                return Ok(await companyRepository.SaveAsync(company.ToCompany()));
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogTrace(CP, "Erro ao salvar dados da empresa", company, companyRepository);
                _logger.LogError(CP, ex, ex.Message, company, companyRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um registro da empresa
        /// </summary>
        /// <param name="contactBook"></param>
        /// <param name="contactBookRepository"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(SaveCompanyRequest company, [FromServices] ICompanyRepository companyRepository)
        {
            _logger.LogInformation("Executando o método para atualizar um registro na agenda", company, companyRepository);
            var CP = 0;

            try
            {
                return Ok(await companyRepository.SaveAsync(company.ToCompany()));
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogTrace(CP, "Erro ao alterar um contato da empresa", company, companyRepository);
                _logger.LogError(CP, ex, ex.Message, company, companyRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um registro da empresa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyRepository"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, [FromServices] ICompanyRepository companyRepository)
        {
            var CP = 0;
            var Mensagem = "";

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "Deletando registro da agenda");
                var result = await companyRepository.DeleteAsync(id);

                Mensagem = result > 0 ? "Registro deletado com sucesso" : "Nenhum registro foi excluido";

                return Ok(Mensagem);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, id, companyRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtem uma lista de registros
        /// </summary>
        /// <param name="companyRepository"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] ICompanyRepository companyRepository)
        {
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, "listando todos registros");

                var response = await companyRepository.GetAllAsync();
                if (!response.Any())
                    return NotFound("Nenhum registro encontrado");

                return Ok(response);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, companyRepository);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtem um registro especifico por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyRepository"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromServices] ICompanyRepository companyRepository)
        {
            var CP = 0;

            try
            {
                CP = 10;
                _logger.LogTrace(CP, $"obter registro por id: {id}");

                var response = await companyRepository.GetAsync(id);
                if (response == null)
                    return NotFound("Nenhum registro encontrado");

                return Ok(response);
            }
            catch (Exception ex)
            {
                CP = 20;
                _logger.LogError(CP, ex, ex.Message, companyRepository);

                return BadRequest(ex.Message);
            }
        }
    }
}