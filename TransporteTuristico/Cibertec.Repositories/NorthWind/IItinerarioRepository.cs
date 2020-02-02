using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface IItinerarioRepository: IRepository<Itinerario>
    {
        Itinerario GetById(int id);
        bool Update(Itinerario itinerario);
        bool Delete(int id);       
        IEnumerable<Itinerario> PagedList(int startRow, int endRow);
        int Count();
        IEnumerable<Itinerario> GetListItinerarios();
    }
}
