using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cibertec.Mvc
{
    public class ColaboradorHub: Hub
    {
        static List<string> colaboradorIds = new List<string>();

        public void AddColaboradorId(string id)
        {
            if (!colaboradorIds.Contains(id)) colaboradorIds.Add(id);
            Clients.All.colaboradorStatus(colaboradorIds);
        }

        public void RemoveColaboradorId(string id)
        {
            if (colaboradorIds.Contains(id)) colaboradorIds.Remove(id);
            Clients.All.colaboradorStatus(colaboradorIds);
        }

        public override Task OnConnected()
        {
            return Clients.All.colaboradorStatus(colaboradorIds);
        }

        public void Message(string message)
        {
            Clients.All.getMessage(message);
        }


    }
}