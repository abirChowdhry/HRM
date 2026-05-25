namespace HRM.Interfaces
{
    public interface ICurrentUserService
    {
        long UserId { get; }
        bool IsAuthenticated { get; }
    }
}
