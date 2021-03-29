using System;
using MagicOnion;
using MagicOnion.Server;
using GrpcService1.Shared;

namespace GrpcService1.Services
{
    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
    {
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine($"Received {x}, {y}");
            return x + y;
        }
    }
}
