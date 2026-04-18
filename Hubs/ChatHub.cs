using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using vedansh.chatservice.DataService;
using vedansh.chatservice.Models;

namespace vedansh.chatservice.Hubs
{
    public class ChatHub:Hub
    {
        private readonly SharedDb _shared;
        public ChatHub(SharedDb shared) => _shared = shared;

        public async Task JoinRoom(UserConnection conn)
        {
            await Clients.All.SendAsync(method:"ReceiveMessage",arg1:"admin",arg2:$"{conn.userName} has joined.");
        }

        public async Task JoinSpecificRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName:conn.chatRoom);
            _shared.connections[Context.ConnectionId]=conn;
            await Clients.Group(conn.chatRoom).SendAsync(method: "JoinSpecificRoom", arg1:"admin",arg2:$"{conn.userName} has joined the room {conn.chatRoom}.");
        }
        public async Task SendMessage(string msg)
        {
            if(_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
            {
                await Clients.Group(conn.chatRoom).SendAsync(method: "ReceiveSpecificMessage", arg1: "admin", arg2: $"{conn.userName} has joined the room {conn.chatRoom}.");
       
               
            }
           
        }
    }
}
