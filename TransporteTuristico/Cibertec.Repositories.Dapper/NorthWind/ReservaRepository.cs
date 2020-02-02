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
    public class ReservaRepository : Repository<Reserva>, IReservaRepository
    {
        public ReservaRepository(string connectionString) : base(connectionString)
        {

        }

        public IEnumerable<Reserva> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Reserva>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Reserva>("dbo.uspReservaPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("select count(*) from Detalle_Reserva where Estado !=0");
                //return connection.ExecuteScalar<int>("select count(*) from Detalle_Reserva");
            }
        }

        public bool GuardarReserva(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Fecha_Inicio", reserva.Fecha_Inicio);
                parameters.Add("@Fecha_Fin", reserva.Fecha_Fin);
                parameters.Add("@IdItinerario", reserva.IdItinerario);
                parameters.Add("@Costo", reserva.Costo);
                parameters.Add("@IdCliente", reserva.IdCliente);
                connection.Execute("dbo.uspClienteReserva", parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public Reserva GetByIdReserva(int getByIdReserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var user = connection.Query<Reserva>("uspSearchReservaById", new { Id = getByIdReserva },
                    commandType: CommandType.StoredProcedure).First();
                return user;
            } 
        }

        public bool ActualizarReserva(Reserva reserva)  
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdReserva", reserva.IdReserva);
                parameters.Add("@Fecha_Inicio", reserva.Fecha_Inicio);
                parameters.Add("@Fecha_Fin", reserva.Fecha_Fin);
                parameters.Add("@IdItinerario", reserva.IdItinerario);
                parameters.Add("@IdCliente", reserva.IdCliente);
                parameters.Add("@NombreCliente", reserva.Nombres);
                parameters.Add("@ApellidoCliente", reserva.Apellidos);
                connection.Execute("dbo.uspClienteUpdate", parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public bool DeleteReserva(int reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdReserva", reserva);
                connection.Execute("dbo.uspClienteDelete", parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public List<Reserva> ListReserva()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var user = connection.Query<Reserva>("uspListeReserva",
                    commandType: CommandType.StoredProcedure).ToList();
                return user;
            }
        }
    }
}
