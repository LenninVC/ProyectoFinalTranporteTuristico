using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface IColaboradorRepository : IRepository<Colaborador>
    {
        Colaborador GetById(int id);
        bool Update(Colaborador colaborador);
        bool Delete(int id);
        IEnumerable<Colaborador> PagedList(int startRow, int endRow);
        int Count();     
    }
}
