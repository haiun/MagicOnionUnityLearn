using System;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;

using GrpcService1.Shared;

namespace GrpcService1.Services
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        private IGroup room;
        private Player player;
        private IInMemoryStorage<Player> storage;
        
        protected override ValueTask OnDisconnected()
        {
            return CompletedTask;
        }
        public async Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Quaternion rotation)
        {
            player = new Player() {Name = userName, Position = position, Rotation = rotation};
            (room, storage) = await Group.AddAsync(roomName, player);
            Broadcast(room).OnJoin(player);
            
            // ReSharper disable once HeapView.BoxingAllocation
            Console.WriteLine($"JoinAsync - {storage.AllValues.Count}");
            
            return storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            Broadcast(room).OnLeave(player);
            await room.RemoveAsync(this.Context);

            // ReSharper disable once HeapView.BoxingAllocation
            Console.WriteLine($"LeaveAsync - {storage.AllValues.Count}");
        }

        public async Task MoveAsync(Vector3 position, Quaternion rotation)
        {
            player.Position = position;
            player.Rotation = rotation;
            Broadcast(room).OnMove(player);
        }

        public async Task SendMessageAsync(string message)
        {
            var response = new MessageResponse {UserName = player.Name, Message = message};
            Broadcast(room).OnSendMessage(response);
            await Task.CompletedTask;
        }
    }
}