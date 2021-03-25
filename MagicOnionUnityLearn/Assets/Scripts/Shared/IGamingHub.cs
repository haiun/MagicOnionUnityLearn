using System.Threading.Tasks;
using MagicOnion;
using UnityEngine;

namespace GrpcService1.Shared
{
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Quaternion rotation);
        Task LeaveAsync();
        Task MoveAsync(Vector3 position, Quaternion rotation);
    }
}