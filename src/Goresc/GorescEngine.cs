namespace Goresc;

public class GorescEngine(ScrappingSettings scrappingSettings)
{
    /// <summary>
    /// Starts a scrapping session which gives access to the Google business page.
    /// </summary>
    /// <returns>The session</returns>
    public async Task<GorescSession> StartAsync()
    {
        // Creates a new browser instance and connects to the business page.
        IGorescBrowser browser = new PuppeteerBrowser(scrappingSettings);
        await browser.ConnectAsync();

        var session = new GorescSession(browser);

        return session;
    }
}