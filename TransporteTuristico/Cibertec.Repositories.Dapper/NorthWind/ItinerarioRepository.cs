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
    class ItinerarioRepository: Repository<Itinerario>, IItinerarioRepository
    {

        public ItinerarioRepository(string connectionString) : base(connectionString)
        {
                
        }

        public Itinerario GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Itinerario>().Where(
                itinerario => itinerario.IdItinerario.Equals(id)).First();
            }
        }

        public bool Update(Itinerario cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = connection.Execute("UPDATE Itinerario SET " +
                                                "IdConductor = @idConductor, " +
                                                "Descripcion = @descripcion, " +
                                                "Origen = @origen,  " +
                                                "Destino = @destino, " +
                                                "Costo = @costo, " +
                                                "Estado = @estado WHERE IdItinerario = @myId",
                                            new
                                            {
                                                idConductor = cliente.IdConductor,
                                                descripcion = cliente.Descripcion,
                                                origen = cliente.Origen,
                                                destino = cliente.Destino,
                                                costo = cliente.Costo,
                                                estado = cliente.Estado,
                                                myId = cliente.IdItinerario
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
                var result = connection.Execute("delete from Itinerario " +
                    "where IdItinerario = @myid ", new { myid = id });

                return Convert.ToBoolean(result);
            }
        }


        public IEnumerable<Itinerario> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Itinerario>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Itinerario>("dbo.uspItinerarioPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(IdItinerario) from dbo.Itinerario");
            }
        }

        public IEnumerable<Itinerario> GetListItinerarios()   
        {
          
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("uspGeListItinerarios", connection);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@OrderId", id);
                List<Itinerario> lstItinerario = new List<Itinerario>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Itinerario itinerario = new Itinerario();
                        itinerario.IdItinerario = Int32.Parse(reader["IdItinerario"].ToString());
                        itinerario.IdConductor = Int32.Parse(reader["IdConductor"].ToString());
                        itinerario.Conductor = reader["Conductor"].ToString();
                        itinerario.Licencia = reader["Licencia"].ToString();
                        itinerario.Descripcion = reader["Descripcion"].ToString();
                        itinerario.Origen = reader["Origen"].ToString();
                        itinerario.Destino = reader["Destino"].ToString();
                        itinerario.Costo = Decimal.Parse(reader["Costo"].ToString());
                        itinerario.Estado = bool.Parse(reader["Estado"].ToString());
                        lstItinerario.Add(itinerario);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return lstItinerario;
            }
        }

    }
}
