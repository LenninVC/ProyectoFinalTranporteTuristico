using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Repositories.NorthWind;
using Cibertec.UnitOfWork;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    public class NorthwindUnitOfWork : IUnitOfWork
    {
        public NorthwindUnitOfWork(string connectionString)
        {
            Customers = new CustomersRepository(connectionString);
            Users = new UserRepository(connectionString);
            Clientes = new ClienteRepository(connectionString);
            Colaboradores = new ColaboradorRepository(connectionString);
            Itinerarios = new ItinerarioRepository(connectionString);
            Reservas = new ReservaRepository(connectionString);
        }

        public ICustomersRepository Customers { get; private set; }
        public IUserRepository Users { get; private set; }
        public IClienteRepository Clientes { get; private set; }
        public IColaboradorRepository Colaboradores { get; private set; }
        public IItinerarioRepository Itinerarios { get; private set; }
        public IReservaRepository Reservas { get; private set; }    
    }
}
