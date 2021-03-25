using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using UnityEngine;
using MagicOnion.Client;
using GrpcService1.Shared;

public class StartUp : MonoBehaviour
{
    void Start()
    {
        Request();
        Request1("A");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("---------END---------");
    }

    async Task Request()
    {
        var channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
        var client = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAsync(100, 200);
        Debug.Log("Client Received " + result);
        
        await channel.ShutdownAsync();
    }

    async Task Request1(string name)
    {
        var channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
        var gamingHubClient = new GamingHubClient();
        
        await gamingHubClient.ConnectAsync(channel, "roomname", name);
        await gamingHubClient.MoveAsync(Vector3.one, Quaternion.identity);
        await gamingHubClient.LeaveAsync();
        await gamingHubClient.DisposeAsync();
        await gamingHubClient.WaitForDisconnect();
        await channel.ShutdownAsync();
    }
}
