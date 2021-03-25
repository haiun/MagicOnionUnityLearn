namespace GrpcService1.Shared
{
    public interface IGamingHubReceiver
    {
        void OnJoin(Player player);
        void OnLeave(Player player);
        void OnMove(Player player);
    }
}