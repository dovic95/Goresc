namespace Goresc.Navigators;

using PuppeteerSharp;

public class DefaultCookiesConsentNavigator : ICookiesConsentNavigator
{
    /// <inheritdoc />
    public async Task AcceptCookiesAsync(IPage page)
    {
        var rejectAllCookiesButton = await page.FindAsync("//button");
        
        if (rejectAllCookiesButton?.Any() == true)
        {
            await rejectAllCookiesButton[0].ClickAsync();
        }

        await page.WaitForNavigationAsync(new NavigationOptions());
    }
}