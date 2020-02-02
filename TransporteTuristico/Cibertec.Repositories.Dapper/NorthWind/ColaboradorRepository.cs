using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Models;
using Cibertec.Repositories.NorthWind;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    class ColaboradorRepository: Repository<Colaborador>, IColaboradorRepository
    {
        public ColaboradorRepository(string connectionString) : base(connectionString)
        {

        }

        public Colaborador GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Colaborador>().Where(
                colaborador => colaborador.IdColaborador.Equals(id)).First();
            }
        }

        public bool Update(Colaborador colaborador)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = connection.Execute("UPDATE Colaborador SET " +
                                                "Documento = @documento, " +
                                                "Nombres = @nombres, " +
                                                "Apellidos = @apellidos,  " +
                                                "Direccion = @direccion, " +
                                                "Telefono = @telefono, " +
                                                "Celular = @celular, " +
                                                "EmailPersonal = @email, " +
                                                "Estado = @estado WHERE IdColaborador = @myId",
                                            new
                                            {
                                                documento = colaborador.Documento,
                                                nombres = colaborador.Nombres,
                                                apellidos = colaborador.Apellidos,
                                                direccion = colaborador.Direccion,
                                                telefono = colaborador.Telefono,
                                                celular = colaborador.Celular,
                                                email = colaborador.EmailPersonal,
                                                estado = colaborador.Estado,
                                                myId = colaborador.IdColaborador
                                            });
                /*
                if (result > 0) return true;
                else return false;
                */
                return Convert.ToBoolean(result);
            }

        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from Colaborador " +
                    "where IdColaborador = @myid ", new { myid = id });

                return Convert.ToBoolean(result);
            }
        }


        public IEnumerable<Colaborador> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Colaborador>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Colaborador>("dbo.uspColaboradorPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(IdColaborador) from dbo.Colaborador");
            }
        }



    }
}
