using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Models;
using Cibertec.Repositories.NorthWind;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    public class ClienteRepository: Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(string connectionString) : base(connectionString)
        {

        }

        public Cliente GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Cliente>().Where(
                cliente => cliente.IdCliente.Equals(id)).First();
            }
        }

        public bool Update(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = connection.Execute("UPDATE Cliente SET " +
                                                "Documento = @documento, " +
                                                "Nombres = @nombres, " +
                                                "Apellidos = @apellidos,  " +
                                                "Direccion = @direccion, " +
                                                "Telefono = @telefono, " +
                                                "Celular = @celular, " +
                                                "Estado = @estado, " +
                                                "Email = @email WHERE IdCliente = @myId",
                                            new
                                            {
                                                documento = cliente.Documento,
                                                nombres = cliente.Nombres,
                                                apellidos = cliente.Apellidos,
                                                direccion = cliente.Direccion,
                                                telefono = cliente.Telefono,
                                                celular = cliente.Celular,
                                                estado = cliente.Estado,
                                                email = cliente.Email,
                                                myId = cliente.IdCliente
                                            });

                return Convert.ToBoolean(result);
            }

        }
        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from Cliente " +
                    "where IdCliente = @myid ", new { myid = id });

                return Convert.ToBoolean(result);
            }
        }

        public IEnumerable<Cliente> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Cliente>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Cliente>("dbo.uspClientePagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(IdCliente) from dbo.Cliente");
            }
        }

        public Cliente GetByDocumento(int documento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Cliente>().Where(
                cliente => cliente.Documento.Equals(documento.ToString())).First();
            }
        }
    }
}
