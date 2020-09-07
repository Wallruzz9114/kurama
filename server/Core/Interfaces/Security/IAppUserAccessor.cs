namespace Core.Interfaces.Security
{
    public interface IAppUserAccessor
    {
        string GetCurrentAppUserUsername();
    }
}