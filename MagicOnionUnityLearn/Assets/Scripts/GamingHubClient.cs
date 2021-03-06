using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcService1.Shared;
using MagicOnion.Client;
using UnityEditor;
using UnityEngine;

public class GamingHubClient : IGamingHubReceiver
{
    private readonly Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private IGamingHub client;

    public async Task<GameObject> ConnectAsync(Channel grpcChannel, string roomName, string playerName)
    {
        client = StreamingHubClient.Connect<IGamingHub, IGamingHubReceiver>(grpcChannel, this);
        var roomPlayers = await client.JoinAsync(roomName, playerName, Vector3.zero, Quaternion.identity);
        foreach (var player in roomPlayers)
        {
            (this as IGamingHubReceiver).OnJoin(player);
        }

        return players[playerName];
    }

    public async Task LeaveAsync()
    {
        await client.LeaveAsync();
    }

    public async Task MoveAsync(Vector3 position, Quaternion rotation)
    {
        await client.MoveAsync(position, rotation);
    }

    public async Task SendMessage()
    {
        await client.SendMessageAsync("Message");
    }

    public async Task DisposeAsync()
    {
        await client.DisposeAsync();
    }

    public async Task WaitForDisconnect()
    {
        await client.WaitForDisconnect();
    }
    
    void IGamingHubReceiver.OnJoin(Player player)
    {
        if (players.ContainsKey(player.Name)) return;
        
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = player.Name;
        cube.transform.SetPositionAndRotation(player.Position, player.Rotation);
        players.Add(player.Name, cube);
    }

    void IGamingHubReceiver.OnLeave(Player player)
    {
        Debug.Log($"OnLeave {player.Name} 1");
        
        if (!players.TryGetValue(player.Name, out var cube))
            return;
        
        GameObject.Destroy(cube.gameObject);
        players.Remove(player.Name);
    }

    void IGamingHubReceiver.OnMove(Player player)
    {
        if (!players.TryGetValue(player.Name, out var cube))
            return;
        
        cube.transform.SetPositionAndRotation(player.Position, player.Rotation);
    }

    void IGamingHubReceiver.OnSendMessage(MessageResponse messageResponse)
    {
        Debug.Log($"{messageResponse.UserName} : {messageResponse.Message}");
    }
}
