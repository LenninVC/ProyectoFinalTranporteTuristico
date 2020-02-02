using Cibertec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cibertec.Repositories.NorthWind
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        IEnumerable<Reserva> PagedList(int startRow, int endRow);
        int Count();
        bool GuardarReserva(Reserva reserva);
        Reserva GetByIdReserva(int getByIdReserva);

        bool ActualizarReserva(Reserva customer);

        bool DeleteReserva(int reserva);

        List<Reserva> ListReserva();
    }
}
