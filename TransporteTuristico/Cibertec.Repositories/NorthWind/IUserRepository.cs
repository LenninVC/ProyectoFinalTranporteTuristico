using Cibertec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cibertec.Repositories.NorthWind
{
    public interface IUserRepository
    {
        Usuario ValidateUser(string email, string password);
        Usuario CreateUser(Usuario user);
    }
}
