namespace Goresc.Navigators;

using PuppeteerSharp;

public interface ICookiesConsentNavigator
{
    /// <summary>
    /// Allows to accept cookies on the Google cookies consent page. 
    /// </summary>
    /// <param name="page">The actual page</param>
    Task AcceptCookiesAsync(IPage page);
}