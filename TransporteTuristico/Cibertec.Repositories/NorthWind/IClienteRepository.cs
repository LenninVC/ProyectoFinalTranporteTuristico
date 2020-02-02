using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface IClienteRepository:IRepository<Cliente>
    {
        Cliente GetById(int id);
        bool Update(Cliente customer);
        bool Delete(int id);
        IEnumerable<Cliente> PagedList(int startRow, int endRow);
        int Count();
       
        Cliente GetByDocumento(int documento);
    }
}
