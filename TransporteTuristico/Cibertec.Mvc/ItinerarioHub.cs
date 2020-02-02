using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cibertec.Mvc
{
    public class ItinerarioHub : Hub
    {
        static List<string> itinerarioIds = new List<string>();

        public void AddItinerarioId(string id)
        {
            if (!itinerarioIds.Contains(id)) itinerarioIds.Add(id);
            Clients.All.itinerarioStatus(itinerarioIds);
        }

        public void RemoveItinerarioId(string id)
        {
            if (itinerarioIds.Contains(id)) itinerarioIds.Remove(id);
            Clients.All.itinerarioStatus(itinerarioIds);
        }

        public override Task OnConnected()
        {
            return Clients.All.itinerarioStatus(itinerarioIds);
        }

        public void Message(string message)
        {
            Clients.All.getMessage(message);
        }
    }
}