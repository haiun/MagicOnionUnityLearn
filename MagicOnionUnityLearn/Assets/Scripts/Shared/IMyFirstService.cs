using System;
using MagicOnion;

namespace GrpcService1.Shared
{
    public interface IMyFirstService : IService<IMyFirstService>
    {
        UnaryResult<int> SumAsync(int x, int y);
    }
}