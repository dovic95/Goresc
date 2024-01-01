namespace Goresc;

public interface IGorescBrowser : IBusinessDataProvider
{
    Task ConnectAsync();
    
    Task DisconnectAsync();
}