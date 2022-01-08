using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.ContactBook;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactBookRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        /// <summary>
        /// Inserindo ou atualiza um registro na base de dados sem precisar de uma instrução sql, apenas similar ao code first do dapper e a classe mapeada
        /// </summary>
        /// <param name="contactBook"></param>
        /// <returns></returns>
        public async Task<IContactBook> SaveAsync(IContactBook contactBook)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new ContactBookDao(contactBook);

            if (dao.Id == 0)
                dao.Id = await connection.InsertAsync(dao);
            else
                await connection.UpdateAsync(dao);

            return dao.Export();
        }

        /// <summary>
        /// Deletando um registro, com uma instrução sql para registrar e passar os parametros, Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            //Quando fica escrito TODO é um ponto incompleto do código que precisa ser trabalhado.
            var sql = "DELETE FROM ContactBook WHERE Id = :id";

            return await connection.ExecuteAsync(sql, new { id });
        }

        /// <summary>
        /// Retorna toda lista de contatos registrados, QueryAsync Dapper
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IContactBook>> GetAllAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook";
            var result = await connection.QueryAsync<ContactBookDao>(query);

            var returnList = new List<IContactBook>();
            returnList.AddRange(result);

            //Não precisa desse trecho, só faz a API pensar mais e demorar para retornar.
            //foreach (var AgendaSalva in result.ToList())
            //{
            //    IContactBook Agenda = new ContactBook(AgendaSalva.Id, AgendaSalva.Name.ToString());
            //    returnList.Add(Agenda);
            //}

            return returnList;
        }

        /// <summary>
        /// Volta apenas um resultado especifico, tuning evitando trazer uma lista na memória e dali filtrar com FirstOrDefault, QuerySingleAsync Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IContactBook> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook WHERE Id = :id";
            var response = await connection.QueryFirstOrDefaultAsync<ContactBookDao>(query, new { id });

            return response;
        }
    }

    [Table("ContactBook")]
    public class ContactBookDao : IContactBook
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ContactBookDao()
        {
        }

        public ContactBookDao(IContactBook contactBook)
        {
            Id = contactBook.Id;
            Name = contactBook.Name;
        }

        public IContactBook Export() => new ContactBook(Id, Name);
    }
}
