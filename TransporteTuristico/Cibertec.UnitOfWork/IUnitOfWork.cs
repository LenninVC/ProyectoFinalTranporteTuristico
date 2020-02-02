using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Repositories.NorthWind;

namespace Cibertec.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICustomersRepository Customers { get; }
        IUserRepository Users { get; }
        IClienteRepository Clientes { get; }
        IColaboradorRepository Colaboradores { get; }
        IItinerarioRepository Itinerarios { get; }
        IReservaRepository Reservas { get; }    
    }
}
