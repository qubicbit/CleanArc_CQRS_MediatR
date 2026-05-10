namespace Application.Common.Interfaces
{
    public interface IUserContextService
    {
        string? UserId { get; }
        bool IsInRole(string role);

    }
}
